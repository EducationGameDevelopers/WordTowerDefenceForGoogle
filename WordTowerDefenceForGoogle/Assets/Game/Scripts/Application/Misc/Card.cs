using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 选择关卡卡片属性
/// </summary>
public class Card
{
    public int LevelID;   //该卡片对应的关卡索引

    public string CardImage;  //卡片名称

    public bool IsLocked = false;   //是否上锁
}

