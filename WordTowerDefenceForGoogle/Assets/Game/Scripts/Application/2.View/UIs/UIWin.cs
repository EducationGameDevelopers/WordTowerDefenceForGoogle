using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIWin:View
{
    #region 字段
    private Text txt_CurrentRound;      //当前波数
    private Text txt_CurrentLevelIndex;    //当前关卡索引
    private Text txt_RateRight;       //答题正确率
    private Text txt_TotalTime;       //答题总时间

    private Button btn_BackSelect;
    private Button btn_ContinueNextLevel;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_UIWin; }
    }
    #endregion

    #region 方法
    /// <summary>
    /// 更新该关卡怪物波数信息
    /// </summary>
    private void UpdateRoundInfo(int currentCount, int levelIndex)
    {
        txt_CurrentRound.text = currentCount.ToString("00");    //当前怪物的波数
        txt_CurrentLevelIndex.text = levelIndex.ToString("00");   //当前关卡索引数
    }

    private void UpdateAnswerMark(float rateRight, float totalTime)
    {
        txt_RateRight.text = rateRight.ToString() + "%";
        txt_TotalTime.text = totalTime.ToString() + " s";
    }

    /// <summary>
    /// 显示该界面
    /// </summary>
    public void Show()
    {
        this.gameObject.SetActive(true);

        GameModel gm = GetModel<GameModel>();
        RoundModel rm = GetModel<RoundModel>();
        UserAnswerModel uam = GetModel<UserAnswerModel>();
        //UI界面显示更新数据
        UpdateRoundInfo(rm.RoundIndex, gm.PlayProgress + 1);
        UpdateAnswerMark(uam.Rate_RightAnswer, uam.Total_AnswerTime);
    }

    /// <summary>
    /// 隐藏该界面
    /// </summary>
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 返回选择关卡按钮点击事件
    /// </summary>
    public void OnBackSelectClick()
    {
        Game.Instance.LoadScene(2);
    }

    /// <summary>
    /// 继续下一个关卡按钮点击事件
    /// </summary>
    public void OnContinueClick()
    {
        GameModel gm = GetModel<GameModel>();
        //当该关卡为最终关卡时，通关
        if (gm.PlayProgress >= gm.LevelList.Count - 1)
        {
            //跳转至通关游戏场景
            Game.Instance.LoadScene(4);
        }
        else
        {
            //开始下一关，StartLevelCommand处理该事件
            SendEvent(Consts.E_StartLevel, new StartLevelArgs() { LevelIndex = gm.PlayProgress + 1 });
        }        
    }
    #endregion

    #region Unity回调
    void Awake()
    {
        txt_CurrentRound = transform.Find("BgPanel/Data/txtCurrentRound").GetComponent<Text>();
        txt_CurrentLevelIndex = transform.Find("BgPanel/Data/txtCurrentLevelIndex").GetComponent<Text>();
        txt_RateRight = transform.Find("BgPanel/AnswerMark/RateRight/Text").GetComponent<Text>();
        txt_TotalTime = transform.Find("BgPanel/AnswerMark/TotalTime/Text").GetComponent<Text>();

        btn_BackSelect = transform.Find("BgPanel/btnBackSelect").GetComponent<Button>();
        btn_ContinueNextLevel = transform.Find("BgPanel/btnContinueNextLevel").GetComponent<Button>();
    }

    private void Start()
    {
        btn_BackSelect.onClick.AddListener(OnBackSelectClick);
        btn_ContinueNextLevel.onClick.AddListener(OnContinueClick);
    }
    #endregion

    #region 事件回调
    public override void HandleEvent(string eventName, object data)
    {

    }
    #endregion

    

   
}

