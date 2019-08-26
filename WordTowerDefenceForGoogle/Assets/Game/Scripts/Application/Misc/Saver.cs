using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 游戏存读类
/// </summary>
public static class Saver
{
    /// <summary>
    /// 读取此时的游戏关卡进度
    /// </summary>
    public static int GetGameProgress()
    {
        return PlayerPrefs.GetInt(Consts.S_GameProgress, 0);
    }

    /// <summary>
    /// 保存(设置)此时的游戏关卡进度
    /// </summary>
    /// <param name="levelIndex"></param>
    public static void SetGameProgress(int levelIndex)
    {
        PlayerPrefs.SetInt(Consts.S_GameProgress, levelIndex);
    }
    /// <summary>
    /// 设置新手教程是否结束
    /// </summary>
    /// <param name="isEnd">isEnd为1表示新手教程结束，为0表示未结束</param>

    public static void SetGuideIsEnd(int isEnd)
    {
        PlayerPrefs.SetInt("isEnd", isEnd);
    }
    /// <summary>
    /// 获取教程是否结束，如果未获取到，默认设为0，0表示未结束，需要进行新手教程
    /// </summary>

    public static int GetGuideIsEnd()
    {
        return PlayerPrefs.GetInt("isEnd", 0);
    }
}

