using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class DaJiTower : ShotTower {

    protected override void Awake()
    {
        base.Awake();
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
        base.SpawnBullet(type, eventObject);
        if (eventObject.name == "generateBullet")
        {
            Game.Instance.a_ObjectPool.ResourcesDir = "Prefabs/Bullets";
            GameObject go = Game.Instance.a_ObjectPool.Spawn("DaJiBullet");
            DaJiBullet bullet = go.GetComponent<DaJiBullet>();
            bullet.transform.position = m_ShotPoint.position;
            bullet.LoadBullet(this.UseBulletID, this.Level, this.MapRect, monster);
        }
    }
}
