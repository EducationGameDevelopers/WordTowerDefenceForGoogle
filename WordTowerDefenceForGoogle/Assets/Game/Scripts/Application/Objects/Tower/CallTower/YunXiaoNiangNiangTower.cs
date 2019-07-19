using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class YunXiaoNiangNiangTower : CallTower {

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
        if (eventObject.name == "generateBullet")
        {
            Game.Instance.a_ObjectPool.ResourcesDir = "Prefabs/Bullets";
            GameObject go = Game.Instance.a_ObjectPool.Spawn("YunXiaoNiangNiangBullet");
            YunXiaoNiangNiangBullet bullet = go.GetComponent<YunXiaoNiangNiangBullet>();
            if (m_Target != null)
            {
                bullet.transform.position = m_Target.transform.position;
                bullet.LoadBullet(this.UseBulletID, this.Level, this.MapRect, monster);
            }

        }
    }
}
