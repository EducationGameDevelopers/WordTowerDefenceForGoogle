using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

/// <summary>
/// 发射子弹的塔
/// </summary>
public class ShotTower : Tower {

    protected UnityEngine.Transform m_ShotPoint;   //子弹的生成点
    //protected Monster monster;
    protected float timer;   //计时器

    protected override void Awake()
    {
        base.Awake();

        m_ShotPoint = transform.Find("ShotPoint");
    }

    protected override void Update()
    {
        base.Update();

    }

    protected override void Attack(Monster monster)
    {
        base.Attack(monster);
    }

    protected override void SpawnBullet(string type, EventObject eventObject)
    {

    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        armatureComponent.AddDBEventListener(EventObject.FRAME_EVENT, SpawnBullet);
        armatureComponent.animation.Play("normal");
    }
}
