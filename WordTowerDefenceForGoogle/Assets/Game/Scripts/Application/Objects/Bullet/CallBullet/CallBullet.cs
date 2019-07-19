using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;
/// <summary>
/// 一切召唤子弹的父类
/// </summary>
public class CallBullet : Bullet {


    //只造成一次伤害，表示是否造成过伤害
    private bool hasDamage = false;

    protected override void Awake()
    {
        base.Awake();

    }
    protected override void Update()
    {
        base.Update();
        if (m_IsExplode)
        {
            return;
        }
        if (Target != null)
        {
            if (Target.isDead == false)
            {
                this.transform.position = Target.transform.position;
                if (hasDamage == false)
                {
                    Target.TakeDamage(this.Damage);
                    hasDamage = true;
                }
            }
            
        }
        
    }
    
    public override void LoadBullet(int bulletID, int level, Rect mapRectRange, Monster target)
    {
        base.LoadBullet(bulletID, level, mapRectRange, target);
    }

    protected void HandleAnimationEnd(string type, EventObject eventObject)
    {
        BulletExplode();
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        armatureComponent.animation.GotoAndPlayByFrame("normal", 0, 1);
        armatureComponent.AddDBEventListener(EventObject.COMPLETE, HandleAnimationEnd);
    }
    
    public override void OnUnspawn()
    {
        base.OnUnspawn();
        armatureComponent.RemoveDBEventListener(EventObject.COMPLETE, HandleAnimationEnd);
        hasDamage = false;
    }

}
