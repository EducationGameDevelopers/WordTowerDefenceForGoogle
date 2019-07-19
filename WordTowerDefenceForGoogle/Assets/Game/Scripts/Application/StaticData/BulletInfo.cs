using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BulletInfo
{
    public int BulletID;

    public string PrefabName;

    public float BaseSpeed;   //子弹的基础速度

    public int BaseDamage;  //子弹的攻击伤害

    public bool IsCanTough;   //子弹是否具有穿透效果
}

