using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Consts
{  
    public static string LevelDir = Application.dataPath + @"/Game/Resources/Levels";  //关卡配置文件路径

    public static readonly string MapDir = Application.dataPath + @"/Game/Resources/Maps";      //地图资源文件路径
    public static readonly string CardDir = Application.dataPath + @"/Game/Resources/Cards";    //选择关卡图片文件路径

    public static readonly string SaveDir = Application.persistentDataPath + "/";   //单词的Json信息存取路径
    public static string WordInitDir = "Words/";

    public static string WordJson_PrimaryDir = "word_primary.json";   //小学单词Json路径
    public static string WordJson_CET4Dir = "CET4.json";   //四级单词Json路径
    public static string WordJson_CET6Dir = "CET6.json";   //六级单词Json路径
    public static string WordJson_PostgraduateDir = "Postgraduate.json";   //考研单词Json路径

    public static string UserData = "user.json";

    //数值
    public const float DotClosedDistance = 0.1f;   //物体间相遇距离
    public const float RangeClosedDistance = 0.7f;

    //存档
    public const string S_GameProgress = "S_GameProgress";  //游戏进度存档

    //Model
    public const string M_GameModel = "M_GameModel";    //游戏数据模型
    public const string M_RoundModel = "M_RoundModel";  //怪物回合信息模型
    public const string M_UserAnswerModel = "M_UserAnswerModel";  //用户数据信息模型

    //View
    public const string V_UIStart = "V_UIStart";
    public const string V_UISelect = "V_UISelect";
    public const string V_UIValcano = "V_UIValcano";
    public const string V_UIBog = "V_UIBog";
    public const string V_UIForest = "V_UIForest";
    public const string V_UISnowMountain = "V_UISnowMountain";
    public const string V_UINight = "V_UINight";
    public const string V_UIDesert = "V_UIDesert";
    public const string V_UIBoard = "V_UIBoard";
    public const string V_UIAnswer = "V_UIAnswer";
    public const string V_UICountDown = "V_UICountDown";
    public const string V_UIWin = "V_UIWin";
    public const string V_UILost = "V_UILost";
    public const string V_UISystem = "V_UISystem";
    public const string V_UIComplete = "V_UIComplete";
    public const string V_UIBloodParent = "V_UIBloodParent";
    public const string V_UISelectLexcion = "UISelectLexcion";

    public const string V_Spawner = "V_Spawner";  //生产怪物
    public const string V_TowerPopup = "V_TowerPopup";  //选择塔的容器界面

    //事件
    public const string E_StartUp = "StartUp";
    public const string E_EnterScene = "EnterScene";  //进入场景事件 SceneArgs
    public const string E_ExitScene = "ExitScene";    //退出场景事件 SceneArgs

    public const string E_StartLevel = "E_StartLevel";  //开始关卡事件 StartArgs
    public const string E_EndLevel = "E_EndLevel";    //结束关卡事件 EndArgs
    public const string E_PauseGame = "E_PauseGame";    //暂停游戏
    public const string E_ContinueGame = "E_ContinueGame";    //继续游戏

    public const string E_CallQuestionPanel = "E_CallQuestionPanel";    //呼出答题界面
    public const string E_LuoboPosition = "E_LuoboPosition";    //萝卜生成的位置 
    public const string E_EndCountDown = "E_EndCountDown";  //计时结束事件 命令

    public const string E_StartRound = "E_StartRound";//开始出怪回合事件 StartRoundArgs
    public const string E_SpawnMonster = "E_SpawnMonster";//出怪事件 SpawnMonsterrgs
    public const string E_RoundInfo = "E_RoundInfo";//当前回合信息

    public const string E_ShowSpawnPanelArgs = "E_ShowSpawnPanelArgs";  //显示设置塔面板 ShowSpawnPanelArgs
    public const string E_ShowUpGradePanelArgs = "E_ShowUpGradePanelArgs";  //显示升级或出售塔面板 ShowUpGradePanelArgs
    public const string E_HideAllPopupsArgs = "E_HideAllPopupsArgs";    //隐藏所有塔容器界面

    public const string E_SpawnTowerArgs = "E_SpawnTowerArgs";  //生成塔事件 SpawnTowerArgs
    public const string E_UpGradeTowerArgs = "E_UpGradeTowerArgs";  //升级塔事件 UpGradeTowerArgs
    public const string E_SellTowerArgs = "E_SellTowerArgs";  //出售塔事件 SellTowerArgs

    public const string E_BloodFollow = "E_BloodFollow";  //血条跟随事件
    public const string E_GoldEffect = "E_GoldEffect";    //金币特效显示事件
    public const string E_DamageNumberText = "E_DamageNumberText"; //显示伤害数字

    public const string E_WordLexcionOption = "E_WordLexcionOption";  //词库选择事件

    public const string E_AnswerMark = "E_AnswerMark";   //用户答题评价事件

    //音乐
    public const string MusicName = "crybaby";
}

/// <summary>
/// 游戏速度模式
/// </summary>
public enum SpeedMode
{
    SpeedOne,
    SpeedTwo
}

/// <summary>
/// 怪物类型
/// </summary>
public enum MonsterType
{
    Monster0,
    Monster1,
    Monster2,
    Monster3,
    Monster4,
    Monster5,
    Monster6,
    Monster7,
    Monster8,
    Monster9,
    Monster10,
    Monster11,
    Monster12
}

/// <summary>
/// 单词词库
/// </summary>
public enum WordLexicon
{
    None,
    Primary,    //小学词库
    CET4,       //四级词库
    CET6,       //六级词库
    Postgraduate,     //考研词库
}

