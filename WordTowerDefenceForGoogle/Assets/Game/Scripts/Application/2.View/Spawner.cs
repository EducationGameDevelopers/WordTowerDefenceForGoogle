using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DragonBones;
using DG.Tweening;
public class Spawner : View
{

    #region 字段
    private Map m_Map = null;   //地图管理类

    private Pet m_Pet = null;  //萝卜类

    private Vector3 m_LuoboPos;   //萝卜的位置
    private Monster monster;

    private CircleShaderController csc;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_Spawner; }
    }
    #endregion

    #region 事件
    #endregion

    #region 方法
    /// <summary>
    /// 生产怪物方法
    /// </summary>
    public void SpawnMonster(string PrefabName)
    {
        //怪物预制体名称
        string prefabName = PrefabName;

        //对象池根据名称加载预制体
        Game.Instance.a_ObjectPool.ResourcesDir = "Prefabs/Monsters";
        GameObject go = Game.Instance.a_ObjectPool.Spawn(prefabName);

        //设置怪物的初始位置
        go.transform.position = m_Map.MonsterSpawnPostion();
        Monster monster = go.GetComponent<Monster>();
        //添加怪物的各种事件
        monster.HpChangeEvent += Monster_HpChangeEvent;
        monster.DeadEvent += Monster_DeadEvent;
        monster.ReachedEvent += Monster_ReachedEvent;

        //血条跟随怪物
        SendEvent(Consts.E_BloodFollow, new BloodFollowArgs() { FollowTarget = go.transform, RelevantMonster = monster });
        //怪物路径加载
        monster.LoadPath(m_Map.Path());
    }

    /// <summary>
    /// 生成终点的宠物
    /// </summary>
    public void SpawnPet()
    {
        GameModel gm = GetModel<GameModel>();
        int levelIndex = Tools.TransformLevelIndex(gm.PlayProgress)[0];
        Game.Instance.a_ObjectPool.ResourcesDir = "Prefabs/Pets";
        GameObject go = null;
        switch (levelIndex)
        {
            case 0:
                go = Game.Instance.a_ObjectPool.Spawn("Phoenix");
                break;
            case 1:
                go = Game.Instance.a_ObjectPool.Spawn("Wolf");
                break;
            case 2:
                go = Game.Instance.a_ObjectPool.Spawn("Tiger");
                break;
            case 3:
                go = Game.Instance.a_ObjectPool.Spawn("Whale");
                break;
            case 4:
                go = Game.Instance.a_ObjectPool.Spawn("Panda");
                break;
            case 5:
                go = Game.Instance.a_ObjectPool.Spawn("Lion");
                break;
        }
        
        go.transform.position = m_Map.LuoBoSpawnPostion();
        m_Pet = go.GetComponent<Pet>();
        //m_LuoboPos = m_Luobo.transform.position;
        //SendEvent(Consts.E_LuoboPosition, new ItemPositionArgs() { ItemPostion = m_LuoboPos });
        //添加萝卜所需触发的事件
        m_Pet.DeadEvent += Pet_DeadEvent;
        m_Pet.HpChangeEvent += Pet_HpChangeEvent;
    }
    /// <summary>
    /// 生成提示用户的用户放置塔的位置
    /// </summary>
    public void SpawnStage(GameModel gModel)
    {
        List<Point> Holder = gModel.PlayLevel.Holder;
        Game.Instance.a_ObjectPool.ResourcesDir = "Prefabs/Stage";
        foreach (Point p in Holder)
        {
            Tile t = new Tile(p.X, p.Y);
            Vector3 position = m_Map.GetGridPostion(t);
            GameObject go = Game.Instance.a_ObjectPool.Spawn("CallStage");
            go.GetComponent<UnityArmatureComponent>().animation.Play("hint");
            go.transform.position = position;
            go.transform.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 生成(放置)塔
    /// </summary>
    public void SpawnTower(int towerID, Vector3 position)
    {
        GameObject[] callStages = GameObject.FindGameObjectsWithTag("CallStage");
        foreach (GameObject stage in callStages)
        {
            if (stage.transform.position == position)
            {
                Game.Instance.a_ObjectPool.Unspawn(stage);
                break;
            }
        }

        //获取生成该塔所在的格子
        Tile tile = m_Map.GetTile(position);

        //获取需创建塔的信息
        TowerInfo info = Game.Instance.a_StaticData.GetTowerInfo(towerID);
        //对象池生成塔
        Game.Instance.a_ObjectPool.ResourcesDir = "Prefabs/Towers";
        GameObject go = Game.Instance.a_ObjectPool.Spawn(info.TowerPrefabName);
        go.transform.position = position;

        //设置塔的信息
        Tower tower = go.GetComponent<Tower>();
        tower.Load(towerID, tile, m_Map.MapRect);
        //在该格子上存放该塔的信息
        tile.Data = tower;

        //金币数减少
        GameModel gm = GetModel<GameModel>();
        gm.Gold -= info.BasePrice;

        if(Game.Instance.isFirst)
        {
            csc = GameObject.Find("Canvas/BG_Dark").GetComponent<CircleShaderController>();
            if(csc.isGuideCallTower)
            {
                //把塔的坐标转换成UI坐标
                Vector3 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, position);

                csc.callImage.transform.position = pos;
                csc.Targets[5] = csc.callImage;
                csc.remindString = RemindString.upGradeOrSellTower;
                csc.ChangeTarget();
            }
        }
        
    }

    #region 宠物的各种事件实现
    /// <summary>
    /// 宠物血量改变
    /// </summary>
    void Pet_HpChangeEvent(int arg1, int arg2)
    {

    }

    /// <summary>
    /// 宠物死亡事件
    /// </summary>
    void Pet_DeadEvent(Role pet)
    {
        //回收宠物
        Game.Instance.a_ObjectPool.Unspawn(pet.gameObject);

        //停止出怪
        GameModel gm = GetModel<GameModel>();
        SendEvent(Consts.E_EndLevel, new EndLevelArgs() { LevelIndex = gm.PlayProgress, IsWin = false });
    }
    #endregion

    #region 怪物的各种事件实现
    void Monster_ReachedEvent(Monster monster)
    {
        monster.Hp = 0;
        //对宠物造成伤害
        m_Pet.TakeDamage(monster.Damage);

        //对象池对该怪物对象进行回收
        Game.Instance.a_ObjectPool.Unspawn(monster.gameObject);

    }
    /// <summary>
    /// 怪物死亡动画结束后调用
    /// </summary>
    //private void AfterDieAnimation(string type, EventObject eventObject)
    //{
    //    //对象池对该怪物对象进行回收
    //    Game.Instance.a_ObjectPool.Unspawn(monster.gameObject);
        
    //    monster.Hp = 0;
    //    monster.GetComponent<UnityArmatureComponent>().RemoveDBEventListener(EventObject.COMPLETE, AfterDieAnimation);

    //    //获取所需的数据模型
    //    GameModel gm = GetModel<GameModel>();
    //    RoundModel rm = GetModel<RoundModel>();
    //    UserAnswerModel uam = GetModel<UserAnswerModel>();

    //    //金钱数增加
    //    gm.Gold += monster.RewardMoney;
    //    monster = null;
    //    //获取当前场景内存在的敌人
    //    GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

    //    //判定该关卡是否胜利
    //    if (m_Pet.IsDead == false              //萝卜没有死亡
    //        && monsters.Length <= 0              //场景内没有怪物
    //        && rm.IsAllRoundComplete == true)    //怪物的波数完成
    //    {
    //        //发送结束关卡事件，传递该关卡胜利信息
    //        SendEvent(Consts.E_EndLevel, new EndLevelArgs() { LevelIndex = gm.PlayProgress, IsWin = true });
    //    }

    //    else if (m_Pet.IsDead == false              //萝卜没有死亡
    //        && monsters.Length <= 0              //场景内没有怪物
    //        && rm.IsAllRoundComplete == false
    //        && rm.CurrentRemainCount <= 0)    //怪物的波数没有完成
    //    {
    //        if (rm.RoundMonsterCount <= 0)
    //        {
                
    //            //下一波怪物可以进来
    //            //rm.StartRound();
    //            //唤出答题界面
    //            SendEvent(Consts.E_CallQuestionPanel);
    //        }
    //    }
    //}

    /// <summary>
    /// 怪物死亡时调用，在Monster类的RoleDead()方法后
    /// </summary>
    /// <param name="monster"></param>
    public void Monster_DeadEvent(Role monster)
    {
        //this.monster = (Monster)monster;
        //UnityArmatureComponent armatureComponent = monster.GetComponent<UnityArmatureComponent>();
        //armatureComponent.AddDBEventListener(EventObject.COMPLETE, AfterDieAnimation);
        //armatureComponent.animation.FadeIn("die", -1, 1);
        Game.Instance.a_ObjectPool.Unspawn(monster.gameObject);

        monster.Hp = 0;
        //monster.GetComponent<UnityArmatureComponent>().RemoveDBEventListener(EventObject.COMPLETE, AfterDieAnimation);

        //获取所需的数据模型
        GameModel gm = GetModel<GameModel>();
        RoundModel rm = GetModel<RoundModel>();
        UserAnswerModel uam = GetModel<UserAnswerModel>();

        //金钱数增加
        gm.Gold += monster.RewardMoney;
        monster = null;
        //获取当前场景内存在的敌人
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        //判定该关卡是否胜利
        if (m_Pet.IsDead == false              //萝卜没有死亡
            && monsters.Length <= 0              //场景内没有怪物
            && rm.IsAllRoundComplete == true)    //怪物的波数完成
        {
            //发送结束关卡事件，传递该关卡胜利信息
            SendEvent(Consts.E_EndLevel, new EndLevelArgs() { LevelIndex = gm.PlayProgress, IsWin = true });
        }

        else if (m_Pet.IsDead == false              //萝卜没有死亡
            && monsters.Length <= 0              //场景内没有怪物
            && rm.IsAllRoundComplete == false
            && rm.CurrentRemainCount <= 0)    //怪物的波数没有完成
        {
            if (rm.RoundMonsterCount <= 0)
            {

                //下一波怪物可以进来
                //rm.StartRound();
                //唤出答题界面
                SendEvent(Consts.E_CallQuestionPanel);
            }
        }
    }

    void Monster_HpChangeEvent(int currentHp, int maxHp)
    {

    }
    #endregion
    #endregion

    #region 事件回调
    public override void RegisterAttentionEvent()
    {
        AttentionEventList.Add(Consts.E_EnterScene);
        AttentionEventList.Add(Consts.E_SpawnMonster);
        AttentionEventList.Add(Consts.E_SpawnTowerArgs);
    }

    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            //进入场景
            case Consts.E_EnterScene:
                SceneArgs e0 = data as SceneArgs;
                if (e0.SceneIndex == 3)
                {
                    //获取模型数据信息
                    GameModel gModel = GetModel<GameModel>();
                    //获取地图编辑类
                    m_Map = gameObject.GetComponent<Map>();

                    m_Map.OnTileClick += Map_OnTileClick;
                    //地图编辑加载信息
                    m_Map.LoadLevel(gModel.PlayLevel);
                    //生成用于提示用户放置塔的台子
                    SpawnStage(gModel);
                    //生成路径终点的萝卜
                    SpawnPet();
                }
                break;

            //出怪事件，RoundModel脚本发出
            case Consts.E_SpawnMonster:
                SpawnMonsterArgs e1 = data as SpawnMonsterArgs;
                //生成怪物
                SpawnMonster(e1.PrefabName);
                break;

            //生成放置塔，TowerPopup脚本发出
            case Consts.E_SpawnTowerArgs:
                SpawnTowerArgs e2 = data as SpawnTowerArgs;
                SpawnTower(e2.TowerID, e2.SpawnPosition);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 地图格子点击事件
    /// </summary>
    void Map_OnTileClick(object sender, TileClickEventArgs e)
    {
        GameModel gm = GetModel<GameModel>();

        Tile tile = e.Tile;

        if (tile.CanHold == false && e.IsOnUI == false)
        {
            //TowerPopup脚本接收处理该事件
            SendEvent(Consts.E_HideAllPopupsArgs);
            return;
        }

        //当前有设置塔的界面显示
        if (TowerPopup.Instance.IsOnShow && e.IsOnUI == false)
        {
            //隐藏所有设置塔的界面，TowerPopup脚本接收处理该事件
            SendEvent(Consts.E_HideAllPopupsArgs);
            return;
        }
        //当游戏正在进行并且这个点击的格子可以放置塔
        if (gm.IsPlaying && tile.CanHold && e.IsOnUI == false)
        {
            //当该格子上没有存放塔的数据时
            if (tile.Data == null)
            {
                //显示选择塔的面板
                ShowSpawnPanelArgs e1 = new ShowSpawnPanelArgs()
                {
                    SpawnPostion = m_Map.GetGridPostion(e.Tile),
                    //显示位置
                    UpSide = tile.Y < (Map.RowCount / 2)
                };
                //发出事件，TowerPopup脚本接收处理该事件
                SendEvent(Consts.E_ShowSpawnPanelArgs, e1);
            }
            else
            {
                Tower tower = tile.Data as Tower;
                //显示升级或出售塔界面
                ShowUpGradePanelArgs e2 = new ShowUpGradePanelArgs()
                {
                    Tower = tower
                };
                //发出事件，TowerPopup脚本接收处理该事件
                SendEvent(Consts.E_ShowUpGradePanelArgs, e2);
            }
        }
        //如果当前该界面为显示状态，先隐藏
        else if (e.IsOnUI == false)
        {
            //发出事件，TowerPopup脚本接收处理该事件
            SendEvent(Consts.E_HideAllPopupsArgs);
        }
    }
    #endregion

    #region 帮助方法
    #endregion

}

