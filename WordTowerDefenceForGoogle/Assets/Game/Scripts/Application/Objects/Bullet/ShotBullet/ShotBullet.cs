using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBullet : Bullet {

    
    //子弹行进的方向
    public Vector3 Direction { get; private set; }

    /// <summary>
    /// 加载子弹
    /// </summary>
    public override void LoadBullet(int bulletID, int level, Rect mapRectRange, Monster target)
    {
        base.LoadBullet(bulletID, level, mapRectRange, target);
        //获取行进方向
        Direction = (Target.transform.position - transform.position).normalized;
    }
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {
        base.Update();
        //子弹爆炸退出
        if (m_IsExplode == true)
            return;

        if (Target != null)
        {
            //目标还没有死亡时
            if (Target.IsDead == false)
            {
                //获取行进方向
                Direction = (Target.transform.position - transform.position).normalized;
                //子弹始终看向目标
                LookAtTarget();
                //子弹向着目标方向移动
                transform.Translate(Direction * Speed * Time.deltaTime, Space.World);
                float distance = Vector3.Distance(Target.transform.position, transform.position);
                //当子弹到达目标位置时
                if (distance <= Consts.DotClosedDistance)
                {
                    //敌人受到伤害
                    Target.TakeDamage(this.Damage);

                    //子弹爆炸
                    BulletExplode();
                }
            }
            else
            {
                BulletExplode();
            }
            
        }
        else
        {
            //子弹向着最后一次获取的方向行进
            transform.Translate(Direction * Speed * Time.deltaTime, Space.World);
            //当子弹没有爆炸且此时子弹已经在当前地图之外
            if (m_IsExplode == false && MapRectRange.Contains(transform.position) == false)
                BulletExplode();
        }

    }

    /// <summary>
    /// 子弹看向目标
    /// </summary>
    protected virtual void LookAtTarget()
    {
        
    }
    public override void OnSpawn()
    {
        base.OnSpawn();
        armatureComponent.animation.Play("normal", 0);
    }
}
