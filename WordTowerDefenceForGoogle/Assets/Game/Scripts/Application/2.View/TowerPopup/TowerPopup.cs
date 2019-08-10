using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using UnityEngine;

public class TowerPopup : View
{
    //单例模式
    private static TowerPopup instance = null;
    private CircleShaderController csc;
    private float timer;  //计时器
     
    public static TowerPopup Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;
        if(Game.Instance.isFirst)
        {
            csc = GameObject.Find("BG_Dark").GetComponent<CircleShaderController>();
        }
    }

    void Start()
    {
        OnHideAllPopups();
    }

    private void Update()
    {
        //当有设置塔界面显示时，默认等待一段时间自动隐藏
        if (IsOnShow == true)
        {
            timer += Time.deltaTime;
            if (timer >= 2)
            {
                OnHideAllPopups();                
            }
        }
    }

    //生成塔的面板
    public SpawnPanel a_SpawnPanel;

    //升级或出售面板
    public UpGradePanel a_UpGradePanel;

    public override string Name
    {
        get { return Consts.V_TowerPopup; }
    }

    //是否有设置塔的界面在显示
    public bool IsOnShow
    {
        get
        {
            if (a_SpawnPanel.gameObject.activeSelf || a_UpGradePanel.gameObject.activeSelf)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    
    /// <summary>
    /// 显示设置塔的面板
    /// </summary>
    public void OnShowSpawnPanel(Vector3 position, bool upSide)
    {
        OnHideAllPopups();
        //对应面板显示
        GameModel gm = GetModel<GameModel>();
        a_SpawnPanel.Show(gm, position, upSide);    
    }

    /// <summary>
    /// 显示升级或出售塔的面板
    /// </summary>
    public void OnShowUpGradePanel(Tower tower)
    {
        OnHideAllPopups();

        //升级面板显示
        a_UpGradePanel.Show(tower);
    }

    /// <summary>
    /// 隐藏所有在塔容器中的面板
    /// </summary>
    public void OnHideAllPopups()
    {       
        a_SpawnPanel.Hide();
        a_UpGradePanel.Hide();
        timer = 0;
    }

    /// <summary>
    /// 生成对应的塔
    /// </summary>
    void OnSpawnTower(object[] objs)
    {
        //获取塔的信息
        int towerID = (int)objs[0];
        Vector3 postion = (Vector3)objs[1];
        //抛出事件，Spawner脚本接收处理该事件
        SendEvent(Consts.E_SpawnTowerArgs, new SpawnTowerArgs() { TowerID = towerID, SpawnPosition = postion });
        OnHideAllPopups();
    }

    /// <summary>
    /// 升级对应的塔
    /// </summary>
    void OnUpGradeTower(Tower tower)
    {
        //抛出事件，UpGradeTowerCommand脚本接收处理该事件
        SendEvent(Consts.E_UpGradeTowerArgs, new UpGradeTowerArgs() { Tower = tower });
        OnHideAllPopups();
    }

    /// <summary>
    /// 出售对应的塔
    /// </summary>
    void OnSellTower(Tower tower)
    {
        GameObject callStage = Game.Instance.a_ObjectPool.Spawn("CallStage");
        callStage.transform.position = tower.transform.position;
        //抛出事件，SellTowerCommand脚本接收处理该事件
        SendEvent(Consts.E_SellTowerArgs, new SellTowerArgs() { Tower = tower });
        OnHideAllPopups();
    }

    #region 事件回调
    public override void RegisterAttentionEvent()
    {
        AttentionEventList.Add(Consts.E_ShowSpawnPanelArgs);
        AttentionEventList.Add(Consts.E_ShowUpGradePanelArgs);
        AttentionEventList.Add(Consts.E_HideAllPopupsArgs);
    }


    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            //显示选择塔的面板
            case Consts.E_ShowSpawnPanelArgs:
                ShowSpawnPanelArgs e1 = data as ShowSpawnPanelArgs;
                OnShowSpawnPanel(e1.SpawnPostion, e1.UpSide);
                break;

            //显示升级或出售塔的面板
            case Consts.E_ShowUpGradePanelArgs:
                ShowUpGradePanelArgs e2 = data as ShowUpGradePanelArgs;
                OnShowUpGradePanel(e2.Tower);
                break;

            //隐藏所有相关面板
            case Consts.E_HideAllPopupsArgs:
                OnHideAllPopups();
                break;

            default:
                break;
        }
    }
    #endregion
}

