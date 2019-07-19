using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using DragonBones;
public class Monster:Role
{
    #region 常量
    #endregion

    #region 事件
    //到达终点事件
    public event Action<Monster> ReachedEvent;
    //public Spawner spawner;
    #endregion

    #region 字段
    public MonsterType MonsterType = MonsterType.Monster0;   //怪物类型
    private float m_MoveSpeed;    //移动速度
    
    private Vector3[] m_PathPoints;    //行进路径坐标点
    private int m_PointIndex = -1;     //下一个点的索引
    private bool m_IsReached = false;  //是否到达了终点
    private int damage;    //该怪物的伤害
    public bool isDead = false;
    private UnityArmatureComponent armatureComponent = null;

    private Vector3 orgionScale;
    private Vector3 newScale;
    #endregion

    #region 属性
    public float MoveSpeed
    {
        get { return m_MoveSpeed; }
        set { m_MoveSpeed = value; }
    }
    private float selfMoveSpeed;

    public int Damage
    {
        get { return damage; }
    }

   
    #endregion

    #region 方法
    /// <summary>
    /// 加载路径坐标点
    /// </summary>
    public void LoadPath(Vector3[] pathPoints)
    {
        m_PathPoints = pathPoints;
    }

    /// <summary>
    /// 是否存在可移动到的下一个坐标点
    /// </summary>
    private bool IsHasNext()
    {
        //下一坐标点是否小于最大坐标点索引
        return (m_PointIndex + 1) < (m_PathPoints.Length - 1);
    }

    private void MoveTo(Vector3 postion)
    {       
        transform.position = postion;
    }

    /// <summary>
    /// 移动到下一个坐标点
    /// </summary>
    private void MoveNextPoint()
    {
        if (IsHasNext() == false)
            return;

        if (m_PointIndex == -1)
        {//当怪物还未处于地图上时，设置初始坐标点
            m_PointIndex = 0;
            MoveTo(m_PathPoints[m_PointIndex]);
        }
        else
        {//怪物存在于地图上时，移动其位置
            m_PointIndex++;
        }
    }


    #endregion

    #region Unity回调
    void Awake()
    {
        armatureComponent = GetComponent<UnityArmatureComponent>();       
        orgionScale = transform.localScale;
        newScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        //spawner = GameObject.Find("Map").GetComponent<Spawner>();
    }
    void Update()
    {
        if (m_IsReached)
            return;
        //当前是否为游戏暂停状态
        if (Game.Instance.IsPauseGame == true)
        {
            StopSelf();
        }
        else
        {
            ContinueSelf();
        }
        if (IsDead)
            return;
        //怪物当前位置
        Vector3 currentPos = transform.position;
        //怪物的目标位置
        Vector3 toPos = m_PathPoints[m_PointIndex + 1];
        //当前位置与目标位置之间的距离
        float distance = Vector3.Distance(toPos, currentPos);

        //怪物向左移动是改变方向
        if (toPos.x < currentPos.x)
        {          
            transform.localScale = newScale;
        }
        else
        {
            transform.localScale = orgionScale;
        }

        if (distance < 0.1f)
        {
            //到达目标位置
            MoveTo(toPos);

            if (IsHasNext() == true)
            {
                //存在可移动到的下一个位置
                MoveNextPoint();
            }
            else
            {
                //到达了终点
                m_IsReached = true;

                //触发终点事件(对萝卜造成伤害)
                if (ReachedEvent != null)
                    ReachedEvent(this);
            }
        }
        else
        {
            //怪物单位移动方向
            Vector3 direction = (toPos - currentPos).normalized;
            //进行帧移动
            transform.Translate(direction * m_MoveSpeed * Time.deltaTime);
        }

    }

    #endregion

    #region 事件回调
    public override void OnSpawn()
    {
        base.OnSpawn();
        armatureComponent.animation.Play("walk");
        //获取怪物信息
        MonsterInfo info = Game.Instance.a_StaticData.GetMonsterInfo((int)MonsterType);
        //信息赋值
        this.MaxHp = info.Hp;
        this.Hp = info.Hp;
        this.m_MoveSpeed = info.MoveSpeed;
        selfMoveSpeed = info.MoveSpeed;

        this.m_RewardMoney = info.RewardMoney;
        this.damage = info.Damage;
        this.isDead = false;
    }

    public override void OnUnspawn()
    {
        base.OnUnspawn();
        //清空事件
        ReachedEvent = null;
        //数据归零
        this.m_MoveSpeed = 0;
        this.m_PathPoints = null;    //行进路径坐标点
        this.m_PointIndex = -1;     //下一个点的索引
        this.m_IsReached = false;  //是否到达了终点
    }

    public override void RoleDead(Role role)
    {
        base.RoleDead(role);
        isDead = true;
    }
    
    #endregion

    #region 帮助方法
    public override void StopSelf()
    {
        MoveSpeed = 0;
    }
    public override void ContinueSelf()
    {
        MoveSpeed = selfMoveSpeed;
    }
    #endregion 
}

