using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 该回合信息
/// </summary>
public class Round
{
    public int MonsterID;   //怪物种类ID

    public int Count;   //怪物数量

    public string PrefabName;   //当前怪物的预制体

    public Round(int monsterID, int count, string PrefabName)
    {
        this.MonsterID = monsterID;
        this.Count = count;
        this.PrefabName = PrefabName;
    }
}

