using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 角色基类
/// </summary>
public abstract class Role : ResuableObject,IReusable
{
    #region 常量
    #endregion

    #region 事件
    public event Action<int, int> HpChangeEvent;   //血量变化事件

    public event Action<Role> DeadEvent;   //死亡事件
    #endregion

    #region 字段
    [SerializeField]
    int m_Hp;     //当前血量

    [SerializeField]
    int m_MaxHp;  //最大血量

    protected int m_RewardMoney;    //该角色所奖励的金币数
    #endregion

    #region 属性
    public int Hp
    {
        get { return m_Hp; }
        set
        {
            //限定血量范围
            value = Mathf.Clamp(value, 0, m_MaxHp);
            
            //防止重复设置血量
            if (value == m_Hp)
                return;

            //血量赋值
            m_Hp = value;
            //血量变化事件触发
            if (HpChangeEvent != null)
                HpChangeEvent(m_Hp, m_MaxHp);
            
            if (m_Hp==0)
            {
                //死亡事件触发
                if (DeadEvent != null)
                    DeadEvent(this);
            }
        }
    }

    public int MaxHp
    {
        get { return m_MaxHp; }
        set 
        {
            if (m_MaxHp < 0)
                return;

            m_MaxHp = value; 
        }
    }

    public bool IsDead
    {
        get { return m_Hp == 0; }
        set
        {
            if(m_Hp == 0)
            {
                IsDead = value;
            }
        }
    }

    public int RewardMoney
    {
        get { return m_RewardMoney; }
    }
    #endregion

    #region 方法
    /// <summary>
    /// 受到伤害
    /// </summary>
    public virtual void TakeDamage(int damage)
    {
        if (IsDead == true)
            return;

        Hp -= damage;       
    }

    /// <summary>
    /// 角色死亡
    /// </summary>
    public virtual void RoleDead(Role role)
    {
      
    }

    public override void OnSpawn()
    {
        //委托死亡事件
        this.DeadEvent += RoleDead;
    }

    public override void OnUnspawn()
    {
        m_Hp = 0;
        m_MaxHp = 0;

        //事件清空
        HpChangeEvent = null;
        DeadEvent = null;
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    #endregion

    #region 帮助方法
    /// <summary>
    /// 暂停自身状态
    /// </summary>
    public virtual void StopSelf() { }

    /// <summary>
    /// 恢复自身状态
    /// </summary>
    public virtual void ContinueSelf() { }

    #endregion
    
}

