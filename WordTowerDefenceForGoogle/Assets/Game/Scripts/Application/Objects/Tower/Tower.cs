using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using DragonBones;

public abstract class Tower : ResuableObject
{
    #region 字段
    protected Animator m_Animator;  //动画状态机

    protected Monster m_Target;     //怪物(该塔所需攻击的目标)

    private Tile m_Tile;    //该塔所对应的格子

    private Vector3 m_OrgionScale;   //塔的初始尺寸

    private int m_Level = 0;        //塔的等级

    private float m_LastAttackTime = 0;   //最后攻击的时间

    protected int shotCount;   //射击次数

    protected UnityArmatureComponent armatureComponent = null;

    protected Monster monster;
    private int m_BasePrice;

    private int m_Price;

    private bool isTopLevel;
    #endregion

    #region 属性
    public int TowerID { get; private set; }
    public int Level
    {
        get { return m_Level; }
        set
        {
            if (m_Level >= MaxLevel - 1)
            {
                isTopLevel = true;
                return;
            }
            else
            {
                isTopLevel = false;
                //等级设置限制
                m_Level = value;
                //塔的大小随它的等级增大
                transform.localScale = transform.localScale * (1 + m_Level * 0.1f);
                //塔的价格设定
                m_Price = m_BasePrice + m_Level * 10;
            }
            
        }
    }

    //该塔是否为顶级
    public bool IsTopLevel { get { return isTopLevel; } }

    public int BasePrice
    {
        get { return m_BasePrice; }
        set { m_BasePrice = value; }
    }

    public int Price { get { return m_Price; } }

    public int MaxLevel { get; private set; }

    public float AttackRange { get; private set; }
    private float selfAttackRange;

    public float ShotRate { get; set; }    //射击速率
    protected float selfShotRate;     //记录固定的射击频率

    public int UseBulletID { get; private set; }

    //塔所在的地图
    public Rect MapRect { get; private set; }

    public Tile Tile
    { 
        get { return m_Tile; }
        set { Tile = value; }
    }
    #endregion

    #region 方法
    public void Load(int towerID, Tile tile,Rect mapRect)
    {
        TowerInfo info = Game.Instance.a_StaticData.GetTowerInfo(towerID);
        m_Tile = tile;
        //读取数据
        this.TowerID = info.TowerID;
        this.MaxLevel = info.MaxLevel;
        Level = 1;

        m_BasePrice = info.BasePrice;
        m_Price = m_BasePrice + Level * 10;
        selfAttackRange = info.AttackRange;
        this.AttackRange = info.AttackRange;
        
        this.ShotRate = info.ShotRate;
        this.selfShotRate = info.ShotRate;

        this.UseBulletID = info.UseBulletID;
        this.MapRect = mapRect;
    }

    /// <summary>
    /// 塔的攻击
    /// </summary>
    protected virtual void Attack(Monster monster)
    {
        this.monster = monster;
        armatureComponent.animation.Play("attack", 1);
    }
    /// <summary>
    /// 动画结束处理事件
    /// </summary>
    /// <param name="type"></param>
    /// <param name="eventObject"></param>
    protected virtual void OnAnimationEventHandler(string type, EventObject eventObject)
    {
        armatureComponent.animation.Play("idle");
    }
    protected virtual void SpawnBullet(string type, EventObject eventObject)
    {

    }
    #endregion

    #region 事件回调
    public override void OnSpawn()
    {
        //m_Animator.Play("Idle");
        m_OrgionScale = transform.localScale;
    }

    public override void OnUnspawn()
    {
        //m_Animator.ResetTrigger("IsAttack");
        m_Target = null;
        
        //回归初始状态
        this.Level = 0;
        m_Level = 0;
        transform.localScale = m_OrgionScale;
        this.MaxLevel = 0;
        m_Price = 0;
        AttackRange = 0;
        ShotRate = 0;
    }
    #endregion

    #region Unity回调
    protected virtual void Awake()
    {
        //m_Animator = transform.GetComponent<Animator>();
        //m_Animator.Play("Idle");
        armatureComponent = GetComponent<UnityArmatureComponent>();
        this.armatureComponent.AddDBEventListener(EventObject.COMPLETE, this.OnAnimationEventHandler);
        this.armatureComponent.animation.Play("idle");
    }

    protected virtual void Update()
    {
        if (Game.Instance.IsPauseGame==true)
        {
            StopSelf();
        }
        else
        {
            ContinueSelf();
        }

        // 塔看向攻击目标
        //if (TowerID != 1)
            //LookAtTarget();

        //没有目标时
        if (m_Target == null)
        {
            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
            foreach (GameObject monster in monsters)
            {
                Monster m = monster.GetComponent<Monster>();
                //目标敌人与该塔的距离
                float distance = Vector3.Distance(monster.transform.position, transform.position);
                //敌人没有死亡且在炮塔的攻击范围之内
                if (m.IsDead == false && distance <= AttackRange)
                {
                    //获得目标
                    m_Target = m;
                    
                    break;
                }
            }
        }
        //找到目标时
        else
        {
            float distance = Vector3.Distance(m_Target.transform.position, transform.position);
            //敌人死亡或在炮塔的攻击范围之外
            if (m_Target.IsDead == true || distance > AttackRange)
            {
                //清空目标
                m_Target = null;
                return;
            }
            
            //攻击间隔时间,每隔ShotRate秒进行一次攻击
            float lastAttackTime = m_LastAttackTime + ShotRate; 
            if (Time.time >= lastAttackTime)
            {          
                
                //发动攻击
                Attack(m_Target);
                //记录最后攻击时间
                m_LastAttackTime = Time.time;

                shotCount++;
            }            
        }
    }

    
    #endregion

    #region 帮助方法
    /// <summary>
    /// 停止自身状态，处于游戏暂停时
    /// </summary>
    public void StopSelf()
    {
        AttackRange = 0;
    }

    /// <summary>
    /// 回复自身状态
    /// </summary>
    public void ContinueSelf()
    {
        AttackRange = selfAttackRange;
    }

    /*塔看向攻击目标
    /// <summary>
    /// 塔看向攻击目标
    /// </summary>
    void LookAtTarget()
    {
        if (m_Target == null)
        {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.z = 0;
            transform.eulerAngles = eulerAngles;
        }
        else
        {
            //塔与目标之间的方向向量
            Vector3 dir = (m_Target.transform.position - transform.position).normalized;
            float dx = dir.x;
            float dy = dir.y;

            //计算方向向量与x轴之间的夹角，
            float angles = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
            //设置塔以z轴旋转相应角度
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.z = angles - 90f;
            transform.eulerAngles = eulerAngles;
        }
    }
    */

    #endregion
}

