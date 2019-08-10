using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using UnityEngine;

public class GameModel:Model
{

    #region 字段
    //关卡集合
    private List<Level> m_LevelList = new List<Level>();

    //当前游戏进度关卡
    private int m_PlayProgress = -1;

    //最大通关的游戏关卡
    private int m_MaxPassProgress = -1;

    //游戏分数(金币)
    private int m_Gold = 0;

    //是否正在游戏中
    private bool m_IsPlaying = false;

    //当前关卡的单词集合
    private static List<Word> m_CurrentLevelWordList = new List<Word>();
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.M_GameModel; }
    }

    public List<Level> LevelList
    {
        get { return m_LevelList; }
    }

    //关卡的最大数量
    public int LevelCount
    {
        get { return m_LevelList.Count; }
    }

    public int PlayProgress
    {
        get { return m_PlayProgress; }
    }

    public int MaxPassProgress
    {
        get { return m_MaxPassProgress; }
    }

    public int Gold
    {
        get { return m_Gold; }
        set 
        {
            m_Gold = value;          
        }
    }

    public bool IsPlaying
    {
        get { return m_IsPlaying; }
        set { m_IsPlaying = value; }
    }
    //当前选择的关卡类
    public Level PlayLevel
    {
        get
        {
            if (m_PlayProgress < 0 || m_PlayProgress > m_LevelList.Count)
                throw new IndexOutOfRangeException("无法找到关卡");

            return m_LevelList[m_PlayProgress];
        }
    }

    public static List<Word> CurrentLevelWordList
    {
        get{ return m_CurrentLevelWordList; }

        set{ m_CurrentLevelWordList = value; }
    }

    #endregion

    #region 方法
    /// <summary>
    /// 初始化
    /// </summary>
    public bool InitGame()
    {        
        //构建关卡集合
        List<FileInfo> files = Tools.GetLevelFiles();
        List<Level> levels = new List<Level>();

        for (int i = 0; i < files.Count; i++)
        {
            Level level = new Level();
            Tools.FillLevel(files[i].Name, ref level);

            levels.Add(level);
        }

        m_LevelList = levels;

        //读取游戏通关进度存档
        //m_MaxPassProgress = Tools.GetMaxProgress();
        //m_MaxPassProgress = Saver.GetGameProgress();
        m_MaxPassProgress = 80;
        //设置初始金币数
        Gold = 400;
        return true;
    }

    /// <summary>
    /// 开始关卡
    /// </summary>
    public void StartLevel(int levelIndex)
    {
        m_PlayProgress = levelIndex;
    }

    /// <summary>
    /// 关卡结束
    /// </summary>
    public void StopLevel(bool isWin)
    {
        //是否需要存储此时的游戏进度
        if (isWin == true && m_PlayProgress >= m_MaxPassProgress)
        {
            //设置存档保存游戏进度
            Saver.SetGameProgress(m_PlayProgress+1);
            //设置此时的最大游戏通关进度
            m_MaxPassProgress = Saver.GetGameProgress();
        }
        m_IsPlaying = false;
        Game.Instance.IsPlayOnLevel = false;
    }

    /// <summary>
    /// 将参与该关卡的单词列表存入Json文件
    /// </summary>
    public void SaveWordListToJson()
    {
        Tools.SaveWordDicToJson(StaticData.m_WordDict);
    }

    /// <summary>
    /// 清空进度存档
    /// </summary>
    public void ClearProgress()
    {
        m_IsPlaying = false;
        m_MaxPassProgress = -1;
        m_PlayProgress = -1;
        //设置存档
        Saver.SetGameProgress(m_MaxPassProgress);
    }


    #endregion
    
}

