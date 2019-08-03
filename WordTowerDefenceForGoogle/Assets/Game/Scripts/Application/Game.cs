using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(ObjectPool))]
[RequireComponent(typeof(Sound))]
[RequireComponent(typeof(StaticData))]
public class Game : ApplicationBase<Game>
{
    //全局访问点
    public ObjectPool a_ObjectPool = null;  //对象池
    public Sound a_Sound = null;    //声音
    public StaticData a_StaticData = null;   //静态数据

    public Text myText;
    public bool IsPlayOnLevel = false;   //是否正在关卡中
    public bool IsPauseGame = false;     //是否暂停游戏
    public bool isFirst = false;

    private SceneLoadProcess m_SceneLoadProcess;

    /// <summary>
    /// 加载场景
    /// </summary>
    public void LoadScene(int sceneLevel)
    {
        SceneArgs e = new SceneArgs();
        //获取当前场景的索引
        e.SceneIndex = SceneManager.GetActiveScene().buildIndex;

        //退出该场景事件发出
        SendEvent(Consts.E_ExitScene, e);

        //加载进度显示场景，进入目标场景
        ShowSceneLoadProcess(sceneLevel);
    }

    /// <summary>
    /// 当加载对应场景完成后
    /// </summary>
    void OnLevelWasLoaded(int sceneLevel)
    {
        SceneArgs e = new SceneArgs();
        e.SceneIndex = sceneLevel;

        //发送进入该场景事件，UICountDown脚本接受并处理
        SendEvent(Consts.E_EnterScene, e);
    }

    void Start()
    {
        Screen.SetResolution(1280, 720, false);
        //该脚本所在物品不会随场景加载而销毁
        GameObject.DontDestroyOnLoad(this.gameObject);
        //全局变量赋值
        a_ObjectPool = ObjectPool.Instance;
        a_Sound = Sound.Instance;
        a_StaticData = StaticData.Instance;

        m_SceneLoadProcess = transform.Find("GameCanvas/UISceneLoadProcess").GetComponent<SceneLoadProcess>();
        HideSceneLoadProcess();

        //注册开始命令
        RegisterController(Consts.E_StartUp, typeof(StartUpCommand));
        //进入游戏（发送命令），StartUpCommand接受处理该事件
        SendEvent(Consts.E_StartUp);        
    }

    public void ShowSceneLoadProcess(int nextSceneIndex)
    {
        m_SceneLoadProcess.gameObject.SetActive(true);
        m_SceneLoadProcess.NextSceneIndex = nextSceneIndex;
        m_SceneLoadProcess.StartSceneLoad();
    }

    public void HideSceneLoadProcess()
    {
        m_SceneLoadProcess.HideSceneLoad();
    }
}
