using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 关卡信息类
/// </summary>
public class Level
{
    public string Name;    //关卡名称

    public string CardImage;  //选择关卡卡片图片

    public string Background;   //背景图片名称

    public string Road;        //路径图片名称

    public int InitScore;       //初始金币

    public int RightCountLimit;   //每轮答题答对数量的上限

    public List<Point> Holder = new List<Point>();   //可放置炮塔的位置

    public List<Point> Path = new List<Point>();     //敌人路径位置

    public List<Round> Rounds = new List<Round>();   //当前关卡敌人信息

    public List<string> WordIds = new List<string>();   //当前关卡的单词

    public List<string> TowerIds = new List<string>();    //当前关卡可以放置的炮塔

    public bool isAble = false; //该关卡是否可以点击，true为可点击，false为不可点击
} 

