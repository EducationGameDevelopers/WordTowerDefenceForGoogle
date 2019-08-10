using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineShotBullet : Bullet {

	public Vector3 Direction { get; private set; }
    protected float distance;
    public virtual void LoadBullet(int bulletID, int level, Rect mapRectRange, Tower tower)
    {
        base.LoadBullet(bulletID, level, mapRectRange, null);
        Direction = tower.transform.right;
    }
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        //if (m_IsExplode)
        //    return;
        //当子弹没有爆炸且此时子弹已经在当前地图之外
        if (m_IsExplode == false && MapRectRange.Contains(transform.position) == false)
            BulletExplode();
        //子弹向英雄朝向行进
        transform.Translate(Direction * Speed * Time.deltaTime, Space.World);
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        armatureComponent.animation.Play("normal", 0);
    }
}
