using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongJiGongZhuBullet : LineShotBullet {

    public override void LoadBullet(int bulletID, int level, Rect mapRectRange, Tower tower)
    {
        base.LoadBullet(bulletID, level, mapRectRange, tower);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Monster monster = collision.gameObject.GetComponent<Monster>();
        if (monster != null && monster.IsDead == false)
        {
            monster.TakeDamage(Damage);
            BulletExplode();
        }
    }
}
