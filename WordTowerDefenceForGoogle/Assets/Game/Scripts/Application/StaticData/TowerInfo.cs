using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TowerInfo
{
    public int TowerID;    //塔的编号ID

    public string TowerPrefabName;  //塔的预制体名称

    public string EnableIcon;   //该塔可选择使用的图标

    public string DisableIcon;  //该塔不可被选择的图标

    public int BasePrice;      //该塔的基础价格

    public int MaxLevel;     //该塔的最大等级

    public float AttackRange;  //该塔的攻击范围

    public float ShotRate;     //塔的设计速度(次/秒)

    public int UseBulletID;   //该塔使用的子弹ID
}

