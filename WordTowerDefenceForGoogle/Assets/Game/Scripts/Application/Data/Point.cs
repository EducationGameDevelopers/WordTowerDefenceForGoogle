using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///地图点的位置类
/// </summary>
public class Point
{
    //位置坐标
    public int X;   //横轴坐标
    public int Y;   //纵轴坐标

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
	
}
