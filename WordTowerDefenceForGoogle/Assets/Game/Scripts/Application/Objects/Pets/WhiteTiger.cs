using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WhiteTiger : Pet
{
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }
    public override void RoleDead(Role role)
    {
        base.RoleDead(role);
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        //获取宠物的数据信息并对其进行赋值
        PetInfo info = Game.Instance.a_StaticData.GetPetInfo(0);
        MaxHp = info.Hp;
        Hp = info.Hp;

        m_UIBoard = GameObject.Find("Canvas/UIBoard").GetComponent<UIBoard>();


        //m_Animator = GetComponent<Animator>();
        //m_Animator.Play("WhiteTiger_Idle");
    }
    public override void OnUnspawn()
    {
        base.OnUnspawn();
    }
}
