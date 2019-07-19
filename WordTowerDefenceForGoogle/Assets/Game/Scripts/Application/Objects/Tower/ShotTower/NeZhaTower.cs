using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DragonBones;
using UnityEngine;

public class NeZhaTower:ShotTower
{

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
        //base.SpawnBullet(type, eventObject);
        if(eventObject.name == "generateBullet")
        {
            //对象池生成子弹对象
            Game.Instance.a_ObjectPool.ResourcesDir = "Prefabs/Bullets";
            GameObject go = Game.Instance.a_ObjectPool.Spawn("NeZhaBullet");
            NeZhaBullet bullet = go.GetComponent<NeZhaBullet>();
            bullet.transform.position = m_ShotPoint.position;
            bullet.LoadBullet(this.UseBulletID, this.Level, this.MapRect, monster);
        }
    }
    IEnumerator WaitShotFinsihed()
    {
        yield return new WaitForSeconds(1);
        //ShotRate = selfShotRate;   //发射频率还原
        shotCount = 0;
    }
    
}

