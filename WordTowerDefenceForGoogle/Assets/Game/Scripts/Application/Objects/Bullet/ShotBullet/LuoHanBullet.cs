using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuoHanBullet : ShotBullet {

    public override void LoadBullet(int bulletID, int level, Rect mapRectRange, Monster target)
    {
        base.LoadBullet(bulletID, level, mapRectRange, target);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }
    protected override void LookAtTarget()
    {
        transform.right = Target.transform.position - transform.position;
    }
}
