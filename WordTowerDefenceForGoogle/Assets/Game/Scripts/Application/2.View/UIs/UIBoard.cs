using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIBoard : View
{
    #region 组件
    private Text txtScore;             //金币数值显示
    private Text txtShowScoreChange;   //金币变化数值
    private Text txtCurrentWave;       //当前波数显示
    private Text txtAllWaves;          //总波数显示
    private Text txtLevelHp;           //当前关卡血量显示

    private Button btnSpeed1;
    private Button btnSpeed2;
    private Button btnPause;
    private Button btnContinue;
    private Button btnSystem;

    private GameObject pauseInfo;       //暂停提示
    public GameObject UISystemObject;    //系统界面

    #endregion

    #region 字段
    private int m_Score;            //分数(金币)
    private SpeedMode m_SpeedMode;  //当前游戏速度
    private bool isPause;           //游戏是否暂停     

    private GameModel m_GM;   //游戏数据模型
    private RoundModel m_RM;  //怪物信息数据模型

    private Color orgionColor;    //金币变化数值的初始颜色
    private Vector2 orgionPos;    //金币变化数值的初始位置
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_UIBoard; }
    }

    public int Score
    {
        get { return m_Score; }
        set
        { 
            m_Score = value;
            txtScore.text = value.ToString();           
        }
    }

    //速度模式，现没有完成
    public SpeedMode SpeedMode
    {
        get { return m_SpeedMode; }
        set 
        { 
            m_SpeedMode = value;
            //速度按钮的显示
            btnSpeed1.gameObject.SetActive(m_SpeedMode == SpeedMode.SpeedOne);
            btnSpeed2.gameObject.SetActive(m_SpeedMode == SpeedMode.SpeedTwo);
        }
    }

    
    #endregion

    #region 方法
    /// <summary>
    /// 更新该关卡怪物波数信息
    /// </summary>
    public void UpdateRoundInfo(int currentCount, int totalCount)
    {
        txtCurrentWave.text = currentCount.ToString().PadLeft(2, '0');
        txtAllWaves.text = totalCount.ToString().PadLeft(2, '0');
    }

    /// <summary>
    /// 速度1按钮点击
    /// </summary>
    public void OnBtnSpeed1Click()
    {
        SpeedMode = SpeedMode.SpeedOne;
    }

    /// <summary>
    /// 速度2按钮点击
    /// </summary>
    public void OnBtnSpeed2Click()
    {
        SpeedMode = SpeedMode.SpeedTwo;
    }

    /// <summary>
    /// 暂停按钮点击
    /// </summary>
    public void OnBtnPauseClick()
    {
        SystemPauseGame(true);
        
    }

    /// <summary>
    /// 继续游戏按钮点击
    /// </summary>
    public void OnBtnContinueClick()
    {
        SystemPauseGame(false);
    }

    /// <summary>
    /// 隐藏游戏设置各元素，如系统按钮，暂停按钮，速度按钮等
    /// </summary>
    public void HideBoardItem()
    {
        btnSystem.gameObject.SetActive(false);
        btnPause.gameObject.SetActive(false);
        btnSpeed1.gameObject.SetActive(false);
        btnSpeed2.gameObject.SetActive(false);
    }

    public void ShowBoardItem()
    {
        btnSystem.gameObject.SetActive(true);

        btnSpeed1.gameObject.SetActive(true);
        btnSpeed2.gameObject.SetActive(true);
    }

    /// <summary>
    /// 游戏暂停与继续设置
    /// </summary>
    /// <param name="isPause">是否为暂停状态</param>
    public void PauseGame(bool isPause)
    {
        Game.Instance.IsPauseGame = isPause;
        //回合暂停
        if (isPause == true)
        {
            m_RM.StopRound();           
        }
        else
        {
            m_RM.StartRound();
            
        }
        ShowPause(isPause);
    }

    public void SystemPauseGame(bool isPause)
    {
        Game.Instance.IsPauseGame = isPause;
        if (isPause == true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        ShowPause(isPause);
    }

    private void ShowPause(bool isPause)
    {
        btnPause.gameObject.SetActive(isPause);
        btnContinue.gameObject.SetActive(!isPause);
        pauseInfo.SetActive(isPause);
    }

    /// <summary>
    /// 系统按钮点击
    /// </summary>
    public void OnBtnSystemClick()
    {
        UISystemObject.GetComponent<UISystem>().Show();
        SystemPauseGame(true);
        
    }

    #endregion

    #region Uinty回调

    public UICountDown uiCountDown;   //倒计时UI
    void Awake()
    {
        m_GM = GetModel<GameModel>();
        m_RM = GetModel<RoundModel>();
               
        txtScore = transform.Find("Score").GetComponent<Text>();
        txtShowScoreChange = txtScore.transform.Find("ShowScoreChange").GetComponent<Text>();
        txtCurrentWave = transform.Find("RoundInfo/txtCurrentWave").GetComponent<Text>();
        txtAllWaves = transform.Find("RoundInfo/txtAllWaves").GetComponent<Text>();
        txtLevelHp = transform.Find("Hp/Text").GetComponent<Text>();

        btnSpeed1 = transform.Find("BtnSpeed1").GetComponent<Button>();
        btnSpeed2 = transform.Find("BtnSpeed2").GetComponent<Button>();
        btnPause = transform.Find("BtnPause").GetComponent<Button>();
        btnContinue = transform.Find("BtnContinue").GetComponent<Button>();
        btnSystem = transform.Find("BtnSystem").GetComponent<Button>();

        pauseInfo = transform.Find("Pauseinfo").gameObject;
        pauseInfo.SetActive(false);
        //倒计时界面开始倒计时
        uiCountDown.StartCountDown();

        //初始化金币数值显示开始状态
        orgionColor = new Color(txtShowScoreChange.color.r, txtShowScoreChange.color.g, txtShowScoreChange.color.b, txtShowScoreChange.color.a);
        orgionPos = txtShowScoreChange.rectTransform.anchoredPosition;

        //游戏为暂停状态
        PauseGame(true);
        //按钮和暂停界面显示
        txtShowScoreChange.gameObject.SetActive(false);
        btnContinue.gameObject.SetActive(true);
        btnPause.gameObject.SetActive(false);

        SpeedMode = SpeedMode.SpeedOne;   

        Score = m_GM.Gold;      //金币数值显示
        UpdateRoundInfo(0, 0);  
    }

    /// <summary>
    /// 该关卡的血量
    /// </summary>
    /// <param name="hp"></param>
    public void HpChange(int hp)
    {       
        txtLevelHp.text = hp.ToString();
    }

    void Update()
    {
        //金币数值变化
        if (Score != m_GM.Gold)
        {
            int changeScore = m_GM.Gold - Score;
            
            StartCoroutine("DelayShowGoldChange", changeScore);      
        }
    }

    /// <summary>
    /// 金币数字改变
    /// </summary>
    /// <param name="changeScore"></param>
    void ShowScoreChange(int changeScore)
    {
        //金币增加会出现音效
        if(Score<m_GM.Gold)
        {
            //实例化拾取金币音效
            Game.Instance.a_Sound.PlayEffectMusic("PickGoldEffect");
        }
        //金币改变
        Score = m_GM.Gold;
        txtShowScoreChange.gameObject.SetActive(true);


        if (changeScore < 0)
            txtShowScoreChange.text = changeScore.ToString();
        else
            txtShowScoreChange.text = "+" + changeScore.ToString();

        //金币数值变化特效
        txtShowScoreChange.rectTransform.DOAnchorPosY(70, 0.8f);
        txtShowScoreChange.DOFade(50, 0.8f).OnComplete(CompleteShowCallBack);
        
    }

    /// <summary>
    /// 金币显示回调
    /// </summary>
    void CompleteShowCallBack()
    {
        txtShowScoreChange.rectTransform.anchoredPosition = orgionPos;
        txtShowScoreChange.color = orgionColor;
        txtShowScoreChange.gameObject.SetActive(false);
    }

    #endregion

    #region 事件回调
    public override void RegisterAttentionEvent()
    {
        this.AttentionEventList.Add(Consts.E_PauseGame);
        this.AttentionEventList.Add(Consts.E_ContinueGame);
        this.AttentionEventList.Add(Consts.E_RoundInfo);
    }

    public override void HandleEvent(string eventName, object date)
    {
        switch (eventName)
        {
            //游戏暂停事件
            case Consts.E_PauseGame:
                PauseGame(true);
                break;

            //继续游戏事件
            case Consts.E_ContinueGame:
                if (Time.timeScale == 0)
                {
                    SystemPauseGame(false);
                }
                else
                {
                    PauseGame(false);
                    ShowBoardItem();
                }
                
                break;
            
            //更新关卡显示UI事件，RoundModel发出
            case Consts.E_RoundInfo:
                RoundInfoArgs info = date as RoundInfoArgs;
                //更新UI的波数信息显示
                UpdateRoundInfo(info.MonsterIndex, info.RoundIndex + 1);
                break;
            default:
                break;
        }
    }
    #endregion

    #region 帮助方法
    /// <summary>
    /// 延迟金币数值变化
    /// </summary>
    IEnumerator DelayShowGoldChange(int changeScore)
    {       
        yield return new WaitForSeconds(0.8f);
        ShowScoreChange(changeScore);
    }
    #endregion
    

    
}
