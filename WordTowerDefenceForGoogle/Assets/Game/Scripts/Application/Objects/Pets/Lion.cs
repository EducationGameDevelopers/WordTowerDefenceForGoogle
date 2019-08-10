using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class Lion : Pet {

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
        //获取宠物的数据消息并对其赋值
        PetInfo info = Game.Instance.a_StaticData.GetPetInfo(5);
        MaxHp = info.Hp;
        Hp = info.Hp;

        m_UIBoard = GameObject.Find("Canvas/UIBoard").GetComponent<UIBoard>();

        this.GetComponent<UnityArmatureComponent>().animation.Play("idle");

    }

    public override void OnUnspawn()
    {
        base.OnUnspawn();
    }
}
