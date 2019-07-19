using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Pet : Role
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    protected Animator m_Animator = null;      //动画状态机

    protected UIBoard m_UIBoard;

    protected int PetID;    //当前宠物的ID
    #endregion


    #region 方法
    /// <summary>
    /// 宠物受到伤害
    /// </summary>
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (IsDead == false)
        {
            m_Animator.SetTrigger("IsDamage");           
        }
        m_UIBoard.HpChange(Hp);
    }

    /// <summary>
    /// 宠物死亡
    /// </summary>
    public override void RoleDead(Role role)
    {
        base.RoleDead(role);

        m_Animator.SetBool("IsDead", true);
    }

    /// <summary>
    /// 宠物的攻击技能
    /// </summary>
    protected virtual void AttackSkill_1() { }

    protected virtual void AttackSkill_2() { }
    #endregion

    #region 事件回调
    public override void OnSpawn()
    {
        base.OnSpawn();
        //获取宠物的数据信息并对其进行赋值
        PetInfo info = Game.Instance.a_StaticData.GetPetInfo(PetID);
        MaxHp = info.Hp;
        Hp = info.Hp;

        m_UIBoard = GameObject.Find("Canvas/UIBoard").GetComponent<UIBoard>();
        m_Animator = GetComponent<Animator>();

        //关卡血量更新显示
        m_UIBoard.HpChange(Hp);       
    }

    public override void OnUnspawn()
    {
        base.OnUnspawn();
        //设置默认动画状态机
        m_Animator.SetBool("IsDead", false);
        m_Animator.ResetTrigger("IsDamage");
    }
    #endregion

    #region Unity回调
    #endregion

    #region 帮助方法
    #endregion
}

