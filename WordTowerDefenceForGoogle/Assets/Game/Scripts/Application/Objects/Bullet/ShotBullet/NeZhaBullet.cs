using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using DragonBones;

public class NeZhaBullet:ShotBullet
{
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
        float angles = Mathf.Atan2(Direction.y, Direction.x);
        Vector3 eulerAngles = transform.eulerAngles;
        eulerAngles.z = angles * Mathf.Rad2Deg - 90;
        transform.eulerAngles = eulerAngles;
    }
}

