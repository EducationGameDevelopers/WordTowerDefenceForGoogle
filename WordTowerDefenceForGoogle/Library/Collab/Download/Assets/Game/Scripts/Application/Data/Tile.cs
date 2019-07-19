using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 地图格子类
/// </summary>
public class Tile
{
    public int X;
    public int Y;

    public bool CanHold = false;   //格子上是否可放置塔

    public object Data;    //格子上存放的数据
    public GameObject hintObject;   //用于提示用户该处可以放塔
    public Tile(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public override string ToString()
    {
        return string.Format("[X:{0},Y:{1},CanHold:{2}]", this.X, this.Y, this.CanHold);
    }
}

