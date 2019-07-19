using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using DragonBones;
/// <summary>
/// 子弹抽象类
/// </summary>
public abstract class Bullet:ResuableObject
{
    #region 字段
    //protected Animator m_Animator;
    protected UnityArmatureComponent armatureComponent;

    protected bool m_IsExplode = false;   //子弹是否爆炸

    public float DelayUpspawnTime = 1f;   //延迟回收的时间(等待爆炸动画完毕)

    protected Vector3 selfScale;

    #endregion

    #region 属性

    public int BulletID { get; private set; }

    //子弹等级
    public int Level { get; set; }

    //子弹是否具有穿透效果
    public bool IsCanTough { get; private set; }

    //子弹基础速度
    public float BaseSpeed { get; private set; }

    //子弹基础伤害
    public int BaseDamage { get; private set; }

    //子弹的真实速度，随等级改变而改变
    public float Speed { get { return BaseSpeed * Level; } }

    //子弹的真实伤害，随等级改变而改变
    public int Damage { get { return BaseDamage * Level; } }

    //当前地图的范围(子弹能活动的范围)
    public Rect MapRectRange { get; private set; }

    //子弹需攻击的目标
    public Monster Target { get; set; }
    #endregion 

    #region 方法
    /// <summary>
    /// 加载子弹
    /// </summary>
    public void Load(int bulletID, int level, Rect mapRectRange)
    {
        MapRectRange = mapRectRange;

        this.BulletID = bulletID;
        this.Level = level;
        //子弹大小随等级增加而增大
        transform.localScale = selfScale * (this.Level * 0.2f + 1);
        
        //获取静态数据并赋值
        BulletInfo info = Game.Instance.a_StaticData.GetBulletInfo(BulletID);
        this.BaseSpeed = info.BaseSpeed;
        this.BaseDamage = info.BaseDamage;
        this.IsCanTough = info.IsCanTough;
    }

    public virtual void LoadBullet(int bulletID, int level, Rect mapRectRange, Monster target)
    {
        Load(bulletID, level, mapRectRange);
        Target = target;
    }

    /// <summary>
    /// 子弹爆炸
    /// </summary>
    protected virtual void BulletExplode()
    {
        m_IsExplode = true;

        //播放爆炸动画
        //m_Animator.SetTrigger("IsExplode");

        //在关卡中执行
        if (Game.Instance.IsPlayOnLevel == true)
            //延迟回收该子弹对象
            StartCoroutine("DelayUpspawnBullet");
        else
            Game.Instance.a_ObjectPool.Unspawn(this.gameObject);
    }

    /// <summary>
    /// 延迟回收子弹
    /// </summary>
    IEnumerator DelayUpspawnBullet()
    {
        //延迟时间
        yield return new WaitForSeconds(1f);

        //对象池回收该子弹对象
        Game.Instance.a_ObjectPool.Unspawn(this.gameObject);
    }
    #endregion

    #region Unity回调
    protected virtual void Awake()
    {
        if (selfScale == Vector3.zero)
            selfScale = transform.localScale;
        armatureComponent = GetComponent<UnityArmatureComponent>();
        
        //m_Animator = transform.GetComponent<Animator>();
        //m_Animator.Play("Play");
    }

    protected virtual void Update()
    {
 
    }
    #endregion

    #region 事件回调
    public override void OnSpawn()
    {
        
    }

    public override void OnUnspawn()
    {
        m_IsExplode = false;
        Target = null;
        //armatureComponent.animation.Reset();
        //m_Animator.ResetTrigger("IsExplode");
    }
    #endregion
}

