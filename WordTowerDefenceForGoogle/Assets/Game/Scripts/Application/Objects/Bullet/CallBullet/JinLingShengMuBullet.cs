using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JinLingShengMuBullet : CallBullet {

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void LoadBullet(int bulletID, int level, Rect mapRectRange, Monster target)
    {
        base.LoadBullet(bulletID, level, mapRectRange, target);
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
    }

    public override void OnUnspawn()
    {
        base.OnUnspawn();
    }
    protected override void BulletExplode()
    {
        base.BulletExplode();
    }
}
