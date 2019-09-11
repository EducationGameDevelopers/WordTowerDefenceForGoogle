using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CircleShaderController : MonoBehaviour
{
    /// <summary>
    /// 要高亮显示的目标
    /// </summary>
    private Image Target;

    private EventPenetrate ev;
    public Text remindText;
    public int currentIndex;
    public Image[] Targets;
    //用于展示提示信息
    public string remindString;
    /// <summary>
    /// 区域范围缓存
    /// </summary>
    private Vector3[] _corners = new Vector3[4];

    /// <summary>
    /// 镂空区域圆心
    /// </summary>
    private Vector4 _center;

    /// <summary>
    /// 镂空区域半径
    /// </summary>
    private float _radius;

    /// <summary>
    /// 遮罩材质
    /// </summary>
    private Material _material;

    /// <summary>
    /// 当前高亮区域的半径
    /// </summary>
    private float _currentRadius;

    /// <summary>
    /// 高亮区域缩放的动画时间
    /// </summary>
    private float _shrinkTime = 0.5f;
    //ChangeTarget方法要用到的canvas
    private Canvas canvas;

    public bool isGuideCallTower = true;       //在答题界面出来前，Game.Instance.isFirst不能设为false,因此需要此flag作为标记表示是建塔处于新手引导中
    public Image callImage;
    /// <summary>
    /// 世界坐标向画布坐标转换
    /// </summary>
    /// <param name="canvas">画布</param>
    /// <param name="world">世界坐标</param>
    /// <returns>返回画布上的二维坐标</returns>
    private Vector2 WorldToCanvasPos(Canvas canvas, Vector3 world)
    {
        Vector2 position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
            world, canvas.GetComponent<Camera>(), out position);
        return position;
    }

    public void ChangeTarget()
    {
        this.gameObject.SetActive(true);
        remindText.text = this.remindString;
        ////循环进行引导
        //if (currentIndex >= Targets.Length)
        //    currentIndex = 0;
        //设置要高亮的目标
        Target = Targets[currentIndex];
        ev.SetTargetImage(Target);

        currentIndex++;
        //Canvas canvas = GameObject.Find("UIStart").GetComponent<Canvas>();
        //获取高亮区域的四个顶点的世界坐标
        Target.rectTransform.GetWorldCorners(_corners);
        //计算最终高亮显示区域的半径
        _radius = Vector2.Distance(WorldToCanvasPos(canvas, _corners[0]),
                      WorldToCanvasPos(canvas, _corners[2])) / 2f;
        //计算高亮显示区域的圆心
        float x = _corners[0].x + ((_corners[3].x - _corners[0].x) / 2f);
        float y = _corners[0].y + ((_corners[1].y - _corners[0].y) / 2f);
        Vector3 centerWorld = new Vector3(x, y, 0);
        Vector2 center = WorldToCanvasPos(canvas, centerWorld);
        //设置遮罩材料中的圆心变量
        Vector4 centerMat = new Vector4(center.x, center.y, 0, 0);
        _material = GetComponent<Image>().material;
        _material.SetVector("_Center", centerMat);
        //计算当前高亮显示区域的半径
        RectTransform canRectTransform = canvas.transform as RectTransform;
        if (canRectTransform != null)
        {
            //获取画布区域的四个顶点
            canRectTransform.GetWorldCorners(_corners);
            //将画布顶点距离高亮区域中心最远的距离作为当前高亮区域半径的初始值
            foreach (Vector3 corner in _corners)
            {
                _currentRadius = Mathf.Max(Vector3.Distance(WorldToCanvasPos(canvas, corner), center),
                    _currentRadius);
            }
        }
        _material.SetFloat("_Slider", _currentRadius);
    }
    private void Awake()
    {
        if(Game.Instance.isFirst == false)
        {
            this.isGuideCallTower = false;
        }
        currentIndex = 0;
        ev = transform.GetComponent<EventPenetrate>();
        remindText = GameObject.Find("Bg/RemindText").GetComponent<Text>();
        if(Game.Instance.isFirst)
        {
            //获取编号
            int index = SceneManager.GetActiveScene().buildIndex;
            switch (index)
            {
                case 1:
                    canvas = GameObject.Find("UIStart").GetComponent<Canvas>();
                    Targets = new Image[2];
                    Targets[0] = GameObject.Find("BtnSelectLexicon").GetComponent<Image>();
                    Targets[1] = GameObject.Find("ButtonStart").GetComponent<Image>();
                    Targets[0].GetComponent<Button>().onClick.AddListener(Hide);
                    currentIndex = 0;
                    this.remindString = RemindString.selectLexicon;
                    //获取画布
                    ChangeTarget();
                    break;
                case 2:
                    canvas = GameObject.Find("UISelect").GetComponent<Canvas>();
                    Targets = new Image[2];
                    Targets[0] = GameObject.Find("Map/map1").GetComponent<Image>();
                    Targets[1] = GameObject.Find("Valcano/Level1/able").GetComponent<Image>();
                    Targets[0].GetComponent<Button>().onClick.AddListener(OnSelectFirstThemeClick);
                    this.remindString = RemindString.selectFirstTheme;
                    ChangeTarget();
                    Targets[1].transform.parent.GetComponent<Button>().onClick.AddListener(Hide);
                    break;
                case 3:
                    canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                    
                    Targets = new Image[8];
                    Targets[0] = GameObject.Find("UIBoard/Hp/HpImageForGuideSys").GetComponent<Image>();
                    Targets[1] = GameObject.Find("UIBoard/Score/ScoreImageForGuideSys").GetComponent<Image>();
                    Targets[2] = GameObject.Find("UIBoard/BtnPause").GetComponent<Image>();
                    this.remindText.text = "";
                    this.transform.Find("Bg").gameObject.SetActive(false);
                    gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                    break;
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 收缩速度
    /// </summary>
    private float _shrinkVelocity = 0f;

    private void Update()
    {
        //从当前半径到目标半径差值显示收缩动画
        float value = Mathf.SmoothDamp(_currentRadius, _radius, ref _shrinkVelocity, _shrinkTime);
        if (!Mathf.Approximately(value, _currentRadius))
        {
            _currentRadius = value;
            _material.SetFloat("_Slider", _currentRadius);
        }
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// 在新手引导中，当点击火山主题按钮后响应
    /// </summary>
    public void OnSelectFirstThemeClick()
    {
        this.remindString = RemindString.selectFirstLevel;
        this.ChangeTarget();
    }
    /// <summary>
    /// 在新手引导中，进入第三个场景时调用此方法，用来等待3秒后进行下一个新手教程提示
    /// </summary>
    public void CountDownCompleted()
    {
        this.transform.Find("Bg").gameObject.SetActive(true);
        this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0.36f);
        this.remindString = RemindString.hpRemind;
        ChangeTarget();
        this.GetComponent<Button>().onClick.AddListener(this.OnClickNext);

    }
    public void OnClickNext()
    {
        if(this.remindString == RemindString.hpRemind)
        {
            this.remindString = RemindString.scoreRemind;
            ChangeTarget();
        }
        else if (this.remindString == RemindString.scoreRemind)
        {
            this.remindString = RemindString.pauseRemind;
            ChangeTarget();
        }
        else if(this.remindString == RemindString.pauseRemind)
        {
            this.GetComponent<Button>().onClick.RemoveListener(this.OnClickNext);
            this.remindString = RemindString.showSpawnPanel;
            GameObject callStage = GameObject.FindGameObjectWithTag("CallStage");
            Vector3 position = RectTransformUtility.WorldToScreenPoint(Camera.main, callStage.transform.position);
            callImage = GameObject.Find("CallStageImage").GetComponent<Image>();
            callImage.transform.position = position;
            //callImage.enabled = false;
            Targets[3] = callImage;
            ChangeTarget();
        }
    }

}