using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RoundModel:Model
{
    #region 常量
    public const float ROUND_INTERVAL = 6f;   //怪物回合之间间隔
    public const float SPAWN_INTERVAL = 2f;   //每个怪物出现的时间间隔
    #endregion

    #region 字段
    private List<Round> m_Rounds = new List<Round>();  //回合信息

    private static int m_RoundIndex = 0;    //当前怪物回合的索引
    private int m_MonsterIndex = 0;  //当前回合的怪物序号

    private bool m_IsAllRoundComplete = false;   //所有怪物是否全部出完  

    private Coroutine m_Coroutine;   //当前波的生产怪物协程

    private int m_CurrentAppearMonsterCount;   //当前波数的剩余怪物数

    private bool isCurrentRoundRunning = false;     //当前波是否正在进行
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.M_RoundModel; }
    }

    public int RoundIndex
    {
        get { return m_RoundIndex; }
    }

    public int MonsetrIndex
    {
        get { return m_MonsterIndex; }
    }

    //怪物回合总数
    public int RoundTotal
    {
        get { return m_Rounds.Count; }
    }

    //当前回合的剩余怪物数
    public int RoundMonsterCount
    {
        get { return m_CurrentAppearMonsterCount; }
    }
    public bool IsAllRoundComplete
    {
        get { return m_IsAllRoundComplete; }
    }

    public bool IsCurrentRoundRunning
    {
        get { return isCurrentRoundRunning; }
        set { isCurrentRoundRunning = value; }
    }
    #endregion

    #region 事件
    #endregion

    #region 方法
    /// <summary>
    /// 加载关卡怪物回合的信息
    /// </summary>
    public void LoadLevel(Level level)
    {
        m_Rounds = level.Rounds;       
    }

    /// <summary>
    /// 开始回合
    /// </summary>
    public void StartRound()
    {
       m_Coroutine = Game.Instance.StartCoroutine(RunRound());
    }

    /// <summary>
    /// 取消(停止)回合--关卡结束或暂停时
    /// </summary>
    public void StopRound()
    {
        if (m_Coroutine == null)
            m_Coroutine = Game.Instance.StartCoroutine(RunRound());

        Game.Instance.StopCoroutine(m_Coroutine);
        m_Coroutine = null;
    }

    /// <summary>
    /// 运行回合(开始出怪)
    /// </summary>
    IEnumerator RunRound()
    {
        //所有回合是否都完成
        if (m_RoundIndex > m_Rounds.Count)
            yield return new WaitForSeconds(0);

        m_IsAllRoundComplete = false;
        isCurrentRoundRunning = true;

        ////构建开始当前回合事件
        //StartRoundArgs e = new StartRoundArgs();
        ////当前事件数据赋值
        //e.RoundTotal = m_Rounds.Count;
        //e.RoundIndex = m_RoundIndex;

        ////发送开始回合事件，
        //SendEvent(Consts.E_StartRound, e);

        Round round = m_Rounds[m_RoundIndex];

        for (int j = m_CurrentAppearMonsterCount; j < round.Count; j++)
        {//每回合出怪
         //发送该波的数据信息事件，UIBoard接受并处理
            SendEvent(Consts.E_RoundInfo, new RoundInfoArgs() { MonsterIndex = j, RoundIndex = m_RoundIndex });
            //每个怪出现的时间间隔
            yield return new WaitForSeconds(SPAWN_INTERVAL);
            //构建出怪事件
            SpawnMonsterArgs ee = new SpawnMonsterArgs();
            //事件赋值
            ee.PrefabName = round.PrefabName;
            //发送生成怪物事件，Spawner接受并处理
            SendEvent(Consts.E_SpawnMonster, ee);

            m_MonsterIndex = j;
            //记录当前怪物数量
            m_CurrentAppearMonsterCount = j;
            //最后一波的最后一个怪完成时不再等待
            if ((m_RoundIndex == m_Rounds.Count - 1) && (j == round.Count - 1))
            {
                //所有怪物出完
                m_IsAllRoundComplete = true;
            }

        }

        //波数增加
        m_RoundIndex++;
        //当前波怪物数归零
        m_CurrentAppearMonsterCount = 0;

        //所有回合还未完成
        if (m_IsAllRoundComplete == false)
        {
            isCurrentRoundRunning = false;  //当前波结束
            //回合之间的时间间隔
            yield return new WaitForSeconds(ROUND_INTERVAL);          
        }

    }

    /// <summary>
    /// 初始化回合数据
    /// </summary>
    public void InitRound()
    {
        m_MonsterIndex = -1;
        m_RoundIndex = 0;
        m_IsAllRoundComplete = false;
    }
    #endregion    

}

