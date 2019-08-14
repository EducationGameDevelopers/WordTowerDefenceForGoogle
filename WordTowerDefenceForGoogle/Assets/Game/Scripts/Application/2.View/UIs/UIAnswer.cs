﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIAnswer:View
{
    #region 字段
    private GameModel m_GM;
    private UserAnswerModel m_UAM;

    private Level m_Level;
    private Game m_Game;

    private Transform m_Canvas;
    private Transform currentClickOption;   //当前点击的选项
    private CoronaDT DT_Corona;    //轮盘动画控制脚本

    //答案选项按钮
    private Button btn_Answer_A;
    private Button btn_Answer_B;
    private Button btn_Answer_C;
    private Button btn_Answer_D;
    private Button btn_back;

    private GameObject GO_WrongOptionReminder;    //选择错误的那个按钮
    private GameObject GO_TimeEndReminder;     //时间结束提示面板
    private GameObject GO_WrongReminderPanel;     //错误提示面板
    private GameObject GO_RightWaittingPanel;     //正确回答等待面板

    private RectTransform rect_QuestionPanel;

    private Text text_QuestionShow;     //问题显示
    private Text text_WordCompleteProgress;     //单词完成度显示
    private Text text_RightAnswerShow;  //正确答案显示
    private Text text_RemindTimerShow;   //提示计时器显示

    private Slider slider_WordCompleteProgress;   //单词完成度进度条

    private int rightIndex;    //正确选项编号  1~4

    private List<Word> wordList;   //总的单词集合
    private List<Word> optionWordList;   //四个选项的单词集合

    private Dictionary<int, Word> wordDict;    //该关卡的单词字典
    private List<int> wordIdList;     //单词的id集合
    private List<int> wrongMarkCountList;   //单词的错误标记数集合

    private int[] optionWordIdArray;   //选项单词的id数组
    private Vector2 pos_Question;     //问题的位置

    private int rewardGold = 50;   //奖励的金币数

    private int chinese_alternate_English = 0;  //中英文问题选项转换

    private int rightCountLimit = 10;   //每轮答题回答正确的上限

    private static Coroutine coroutine_timer;   //计时器地协程

    private int remindTimer = 11;    //提示计时器，一般为倒计时10秒
    private System.Random random;   //随机数生成类

    public GameObject RightLimitReminder;
    private int orgionData;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_UIAnswer; }
    }
    #endregion

    #region Unity回调方法 Awake() Start()
    void Awake()
    {
        m_GM = GetModel<GameModel>();
        m_UAM = GetModel<UserAnswerModel>();
        m_UAM.InitData();

        m_Level = m_GM.PlayLevel;
        m_Game = Game.Instance;

        rightCountLimit = m_Level.RightCountLimit;
        orgionData = rightCountLimit;

        random = new System.Random();

        m_Canvas = transform.parent.transform;
        DT_Corona = transform.Find("AnwserPanel").GetComponent<CoronaDT>();

        btn_Answer_A = transform.Find("AnwserPanel/Bg_Corona/AnswerOption_1").GetComponent<Button>();
        btn_Answer_B = transform.Find("AnwserPanel/Bg_Corona/AnswerOption_2").GetComponent<Button>();
        btn_Answer_C = transform.Find("AnwserPanel/Bg_Corona/AnswerOption_3").GetComponent<Button>();
        btn_Answer_D = transform.Find("AnwserPanel/Bg_Corona/AnswerOption_4").GetComponent<Button>();

        btn_Answer_A.transform.Find("WrongReminder").gameObject.SetActive(false);
        btn_Answer_B.transform.Find("WrongReminder").gameObject.SetActive(false);
        btn_Answer_C.transform.Find("WrongReminder").gameObject.SetActive(false);
        btn_Answer_D.transform.Find("WrongReminder").gameObject.SetActive(false);

        btn_back = transform.Find("BackBtn").GetComponent<Button>();

        rect_QuestionPanel = transform.Find("QuestionPanel").GetComponent<RectTransform>();
        text_QuestionShow = transform.Find("QuestionPanel/Text").GetComponent<Text>();
        text_RemindTimerShow = transform.Find("RemindTimer").GetComponent<Text>();
        text_WordCompleteProgress = transform.Find("QuestionPanel/WordCompleteProgressBar/ShowText/txtProgressShow").GetComponent<Text>();

        slider_WordCompleteProgress = transform.Find("QuestionPanel/WordCompleteProgressBar").GetComponent<Slider>();

        GO_WrongReminderPanel = transform.Find("WrongReminderPanel").gameObject;
        GO_RightWaittingPanel = transform.Find("RightWaittingPanel").gameObject;
        text_RightAnswerShow = GO_WrongReminderPanel.transform.Find("RightAnswerPanel/AnswerText").GetComponent<Text>();
        GO_TimeEndReminder = transform.Find("TimeEndReminder").gameObject;

        slider_WordCompleteProgress.value = 0;
        GO_TimeEndReminder.SetActive(false);
        GO_WrongReminderPanel.SetActive(false);
        GO_RightWaittingPanel.SetActive(false);
        RightLimitReminder.SetActive(false);
        btn_back.onClick.AddListener(OnBackBtnOnClick);
    }

    void Start()
    {
        InitWordList();   //初始化单词集合
        SetOption();      //设置选项
    }

    #endregion

    #region 单词选择机制
    /// <summary>
    /// 初始化该关卡的单词列表
    /// </summary>
    void InitWordList()
    {
        wordDict = new Dictionary<int, Word>();
        wordIdList = new List<int>();
        wrongMarkCountList = new List<int>();
        wordList = new List<Word>();

        foreach (string wordId in m_Level.WordIds)
        {            
            Word word = m_Game.a_StaticData.GetWord(wordId);

            wordDict.Add(int.Parse(word.Str_Id), word);
            wordIdList.Add(int.Parse(word.Str_Id));
            wrongMarkCountList.Add(word.WrongMarkCount);

            wordList.Add(word);
        }
        //模型数据中记录该单词列表       
    }
/* //常规随机抽取单词方案
 
    /// <summary>
    /// 从单词集合中提取一个单词
    /// </summary>
    Word ExtractWord()
    {

        int index = UnityEngine.Random.Range(0, wordList.Count - 1);
        return wordList[index];
    }

    /// <summary>
    /// 该单词在选项单词集合中是否重复
    /// </summary>
    bool isRepetition(Word word)
    {
        if (optionWordList == null)
            return true;

        foreach (Word item in optionWordList)
        {
            if (word.Str_Id == item.Str_Id)
                return false;
        }

        return true;
    }
*/

    /// <summary>
    /// 获取选项单词集合
    /// </summary>
    void GetOptionWords()
    {
        optionWordList = new List<Word>();

        //根据单词的错误标记数选出当前出现频率最高的四个选项
        optionWordIdArray = MathTools.ControllerRandomExtract(random, wordIdList, wrongMarkCountList, 4);


        for (int i = 0; i < optionWordIdArray.Length; i++)
        {
            optionWordList.Add(wordDict[optionWordIdArray[i]]);
        }
    }
    #endregion

    #region 选项设置
    /// <summary>
    /// 设置正确选项内容
    /// </summary>
    void SetRightOptionContent(Button option, string English, string Chinese)
    {
        option.onClick.AddListener(()=> { RightOptionOnClick(option.transform); } );

        //问题为中文，选项为英文
        if (chinese_alternate_English % 2 == 0)
        {
            option.transform.GetComponentInChildren<Text>().text = English;
            //问题面板显示正确选项的中文
            text_QuestionShow.text = Chinese;
            //播放该单词的中文发音
            Game.Instance.a_Sound.PlayChinesePronunciation(English);
        }
        //问题为英文，选项为中文
        else
        {
            option.transform.GetComponentInChildren<Text>().text = Chinese;
            //问题面板显示正确选项的英文
            text_QuestionShow.text = English;
            //播放该单词的英文发音
            Game.Instance.a_Sound.PlayEnglishPronunciation(English);
        }
        
    }

    /// <summary>
    /// 设置错误选项内容
    /// </summary>
    void SetWrongOptionContent(Button option, string English,string Chinese)
    {
        //错误选项添加带参的回调
        option.onClick.AddListener(delegate () 
        {
            WrongOptionOnClick(option.transform);
        });

        if (chinese_alternate_English % 2 == 0)
        {
            option.transform.GetComponentInChildren<Text>().text = English;
        }
        else
        {
            option.transform.GetComponentInChildren<Text>().text = Chinese;
        }
        
    }

    /// <summary>
    /// 设置选项
    /// </summary>
    void SetOption()
    {
        ClearOnClick();      //清除按钮上的点击事件

        remindTimer = 10;
        text_RemindTimerShow.text = remindTimer.ToString();

        //重新获取单词错误数集合
        wrongMarkCountList = new List<int>();

        if (wordDict.Count!= 0)
        {
            foreach (Word word in wordDict.Values)
            {
                wrongMarkCountList.Add(word.WrongMarkCount);
            }
        }
     
        GetOptionWords();    //获取选项单词

        rightIndex = UnityEngine.Random.Range(1, 4);
        if (rightIndex == 1)
        {
            SetRightOptionContent(btn_Answer_A, optionWordList[0].Str_English, optionWordList[0].Str_Chinese);
            SetWrongOptionContent(btn_Answer_B, optionWordList[1].Str_English, optionWordList[1].Str_Chinese);
            SetWrongOptionContent(btn_Answer_C, optionWordList[2].Str_English, optionWordList[2].Str_Chinese);
            SetWrongOptionContent(btn_Answer_D, optionWordList[3].Str_English, optionWordList[3].Str_Chinese);
        }

        else if (rightIndex == 2)
        {
            SetRightOptionContent(btn_Answer_B, optionWordList[0].Str_English, optionWordList[0].Str_Chinese);
            SetWrongOptionContent(btn_Answer_A, optionWordList[1].Str_English, optionWordList[1].Str_Chinese);
            SetWrongOptionContent(btn_Answer_C, optionWordList[2].Str_English, optionWordList[2].Str_Chinese);
            SetWrongOptionContent(btn_Answer_D, optionWordList[3].Str_English, optionWordList[3].Str_Chinese);
        }

        else if (rightIndex == 3)
        {
            SetRightOptionContent(btn_Answer_C, optionWordList[0].Str_English, optionWordList[0].Str_Chinese);
            SetWrongOptionContent(btn_Answer_B, optionWordList[1].Str_English, optionWordList[1].Str_Chinese);
            SetWrongOptionContent(btn_Answer_A, optionWordList[2].Str_English, optionWordList[2].Str_Chinese);
            SetWrongOptionContent(btn_Answer_D, optionWordList[3].Str_English, optionWordList[3].Str_Chinese);
        }

        else if (rightIndex == 4)
        {
            SetRightOptionContent(btn_Answer_D, optionWordList[0].Str_English, optionWordList[0].Str_Chinese);
            SetWrongOptionContent(btn_Answer_B, optionWordList[1].Str_English, optionWordList[1].Str_Chinese);
            SetWrongOptionContent(btn_Answer_C, optionWordList[2].Str_English, optionWordList[2].Str_Chinese);
            SetWrongOptionContent(btn_Answer_A, optionWordList[3].Str_English, optionWordList[3].Str_Chinese);
        }
        chinese_alternate_English++;

        coroutine_timer = StartCoroutine(StartSelectAnswer());
    }
    #endregion

    #region 按钮单击事件
    /// <summary>
    /// 清空选项的点击事件
    /// </summary>
    void ClearOnClick()
    {
        btn_Answer_A.onClick.RemoveAllListeners();
        btn_Answer_B.onClick.RemoveAllListeners();
        btn_Answer_C.onClick.RemoveAllListeners();
        btn_Answer_D.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// 正确选项的点击事件
    /// </summary>
    void RightOptionOnClick(Transform option)
    {
        //答题总点击数和正确点击数增加
        m_UAM.Total_ClickCount += 1;
        m_UAM.Total_RightClickCount += 1;
        rightCountLimit -= 1;

        if (rightCountLimit <= 0)
        {
            RightLimitReminder.SetActive(true);
        }
        else
        {
            //在规定时间内答对获得全部奖励，否则只有一半
            if (remindTimer >= 5)
            {
                //获得金币
                m_GM.Gold += rewardGold;
            }
            else
            {
                m_GM.Gold += (int)(rewardGold / 2);
            }

            //生成金币特效
            for (int i = 0; i < 5; i++)
            {
                //在答案位置生成金币
                Vector3 goldPosition = new Vector3(100, 20 + i * 80, 0);

                //金币坐标信息
                ItemPositionArgs goldPosArgs = new ItemPositionArgs() { ItemPostion = goldPosition };
                //发出事件，UIBloodParent脚本接受并处理
                SendEvent(Consts.E_GoldEffect, goldPosArgs);
            }
        }
        

        if (coroutine_timer != null)
        {
            //停止计时器协程
            StopCoroutine(coroutine_timer);
            coroutine_timer = null;
        }

        //选项轮盘动画播放
        DT_Corona.OptionDT(option, true);

        //本地数据记录正确数增加
        StaticData.ModityWordMarkCount(optionWordList[0].Str_Id, true);
        //当前单词字典中的对应单词错误标记减一
        wordDict[int.Parse(optionWordList[0].Str_Id)].WrongMarkCount -= 1;
        //等待发音完毕
        StartCoroutine(WaitCompletePronunication());
    }

    /// <summary>
    /// 错误选项的点击事件
    /// </summary>
    void WrongOptionOnClick(Transform option = null)
    {
        m_UAM.Total_ClickCount += 1;

        if (option != null)
        {
            GO_WrongOptionReminder = option.Find("WrongReminder").gameObject;
        }
        else
        {
            GO_WrongOptionReminder = GO_TimeEndReminder;
        }
        currentClickOption = option;

        //停止计时器协程
        StopCoroutine(coroutine_timer);
        coroutine_timer = null;

        //出现错误提示板，该单词错误标记数加一
        StartCoroutine(WrongIndictionShowTime());

        //本地数据记录错误数增加
        StaticData.ModityWordMarkCount(optionWordList[0].Str_Id, false);
        //当前单词字典中的对应单词错误标记加一
        wordDict[int.Parse(optionWordList[0].Str_Id)].WrongMarkCount += 2;
        //播放中英文发音      
        StartCoroutine(WaitPronunationFinished());
    }

    /// <summary>
    /// 返回游戏界面按钮点击
    /// </summary>
    private void OnBackBtnOnClick()
    {
        HideSelf();
        SendEvent(Consts.E_ContinueGame);
    }
    #endregion

    #region 协程方法
    /// <summary>
    /// 等待正确答案提示
    /// </summary>
    IEnumerator WaitCompletePronunication()
    {
        if (chinese_alternate_English % 2 == 0)
        {
            //播放当前单词的发音
            Game.Instance.a_Sound.PlayChinesePronunciation(optionWordList[0].Str_English);
        }
        else
        {
            //播放当前单词的发音
            Game.Instance.a_Sound.PlayEnglishPronunciation(optionWordList[0].Str_English);
        }

        GO_RightWaittingPanel.SetActive(true);

        yield return new WaitForSeconds(2.7f);

        GO_RightWaittingPanel.SetActive(false);
        RightLimitReminder.SetActive(false);
        SetOption();   //正确答案提示后重新设置选项
    }


    /// <summary>
    /// 回答错误,显示错误提示面板,一段时间后隐藏
    /// </summary>
    IEnumerator WrongIndictionShowTime()
    {
        //错误提示面板显示
        GO_WrongReminderPanel.SetActive(true);
        GO_WrongOptionReminder.SetActive(true);
        //显示正确答案
        text_RightAnswerShow.text = optionWordList[0].Str_Chinese + "    " + optionWordList[0].Str_English;

        if (currentClickOption != null)
            DT_Corona.OptionDT(currentClickOption, false);

        yield return new WaitForSeconds(2.5f);

        //重新启动选择答题倒计时
        coroutine_timer = StartCoroutine(StartSelectAnswer());
        

        GO_WrongReminderPanel.SetActive(false);
        GO_WrongOptionReminder.SetActive(false);
    }

    
    /// <summary>
    /// 选择答案开始倒计时
    /// </summary>
    IEnumerator StartSelectAnswer()
    {
        remindTimer = 11;
        while (remindTimer > 0)
        {
                      
            remindTimer -= 1;
            text_RemindTimerShow.text = remindTimer.ToString();
            m_UAM.Total_AnswerTime += 1;
            yield return new WaitForSeconds(1);

            //倒计时结束时，结果为错误答案
            if (remindTimer <= 0)
            {
                WrongOptionOnClick();               
                break;
            }
        }
        yield return 0;
    }

    
    /// <summary>
    /// 等待发音完毕
    /// </summary>
    IEnumerator WaitPronunationFinished()
    {
        //播放当前单词中文的发音
        Game.Instance.a_Sound.PlayChinesePronunciation(optionWordList[0].Str_English);
        yield return new WaitForSeconds(1);
        //播放当前单词英文的发音
        Game.Instance.a_Sound.PlayEnglishPronunciation(optionWordList[0].Str_English);
    }

    public void ShowSelf()
    {
        this.gameObject.SetActive(true);
    }

    public void HideSelf()
    {
        //轮盘重置
        DT_Corona.RemodelCorona();

        this.gameObject.SetActive(false);

        //暂停答题计时器协程
        StopAllCoroutines();
        
        GO_RightWaittingPanel.SetActive(false);
        GO_WrongReminderPanel.SetActive(false);
        RightLimitReminder.SetActive(false);

        rightCountLimit = orgionData;

        if (GO_WrongOptionReminder != null)
            GO_WrongOptionReminder.SetActive(false);
    }
    #endregion

    #region 事件回调

    public void CallSelf()
    {
        ShowSelf();
        if (wordDict != null)
            SetOption();   //唤出单体界面时重新设置选项  
    }

    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            //宠物位置事件处理，现已不用
            case Consts.E_LuoboPosition:
                ItemPositionArgs e = data as ItemPositionArgs;
                //将萝卜的坐标转为视口坐标
                Vector2 srePos = Camera.main.WorldToScreenPoint(e.ItemPostion);
       
                RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Canvas as RectTransform, srePos, m_Canvas.GetComponent<Camera>(), out pos_Question);
         
                //问题出现在宠物上方，现已不用
                pos_Question.y += 80;
                rect_QuestionPanel.anchoredPosition = pos_Question;
                break;

            default:
                break;
        }
    }

    public override void RegisterAttentionEvent()
    {
        //该视图关心萝卜的位置
        this.AttentionEventList.Add(Consts.E_LuoboPosition);
        this.AttentionEventList.Add(Consts.E_CallQuestionPanel);
    }
    #endregion
        
}

