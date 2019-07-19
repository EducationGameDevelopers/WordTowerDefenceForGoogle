using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

/// <summary>
/// 所有召唤魔法阵的塔的父类
/// </summary>
public class CallTower : Tower {

    #region 字段
    private Vector3 bulletPosition; //子弹的生成点
    public Vector3 BulletPosition
    {
        get
        {
            bulletPosition = this.m_Target.transform.position;
            return bulletPosition;
        }
    }
    #endregion

    #region Unity回调
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

    }
    #endregion
    #region 方法
    protected override void Attack(Monster monster)
    {
        base.Attack(monster);
    }

    protected override void SpawnBullet(string type, EventObject eventObject)
    {
        
    }
    public override void OnUnspawn()
    {
        base.OnUnspawn();
    }
    public override void OnSpawn()
    {
        base.OnSpawn();
        armatureComponent.AddDBEventListener(EventObject.FRAME_EVENT, SpawnBullet);
    }
    #endregion
}
