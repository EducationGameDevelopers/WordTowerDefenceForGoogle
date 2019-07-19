using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 静态数据类
/// </summary>
public class StaticData : Singleton<StaticData>
{
    //怪物ID与怪物信息的映射
    private Dictionary<int, MonsterInfo> m_MonsterDict = new Dictionary<int, MonsterInfo>();
    //宠物数据
    private Dictionary<int, PetInfo> m_PetsDict = new Dictionary<int, PetInfo>();
    //塔的数据
    private Dictionary<int, TowerInfo> m_TowerDict = new Dictionary<int, TowerInfo>();
    //子弹的数据
    private Dictionary<int, BulletInfo> m_BulletDict = new Dictionary<int, BulletInfo>();

    //单词字典
    public static Dictionary<string, Word> m_WordDict = new Dictionary<string, Word>();


    private WordLexicon wordLexicon = WordLexicon.None;    //单词词库

    private WordLexicon wordLexiconOption = WordLexicon.None;

    public WordLexicon WordLexiconOption
    {
        get { return wordLexiconOption; }
    }

    protected override void Awake()
    {
        base.Awake();
       
        InitPet();
        InitMonster();
        InitTower();
        InitBullet();
        InitPersistentWordFiles();
    }

    /// <summary>
    /// 选择单词词库
    /// </summary>
    public void SelectWordLexcion(WordLexicon wordLexicon)
    {
        this.wordLexicon = wordLexicon;

        //选择词库前先情况单词字典
        ClearWordDict();
        //选择JSON词库
        switch (this.wordLexicon)
        {
            case WordLexicon.None:
                break;

            case WordLexicon.Primary:
                InitWordDict(Consts.SaveDir + Consts.WordJson_PrimaryDir);          
                break;

            case WordLexicon.CET4:
                InitWordDict(Consts.SaveDir + Consts.WordJson_CET4Dir);
                break;

            case WordLexicon.CET6:
                InitWordDict(Consts.SaveDir + Consts.WordJson_CET6Dir);
                break;

            case WordLexicon.Postgraduate:
                InitWordDict(Consts.SaveDir + Consts.WordJson_PostgraduateDir);
                break;

            default:
                break;
        }

        //本地化存储词库选项
        PlayerPrefs.SetString("WordLexcionPotion", wordLexicon.ToString());
        //本地取出该选项记录
        wordLexiconOption = (WordLexicon)Enum.Parse(typeof(WordLexicon), PlayerPrefs.GetString("WordLexcionPotion"));
    }

    /// <summary>
    /// 初始化宠物信息
    /// </summary>
    private void InitPet()
    {
        Tools.AnalysisPet(ref m_PetsDict);
    }

    /// <summary>
    /// 初始化怪物信息
    /// </summary>
    private void InitMonster()
    {
        Tools.AnalysisMonster(ref m_MonsterDict);
    }

    /// <summary>
    /// 初始化塔的信息
    /// </summary>
    private void InitTower()
    {
        Tools.AnalysisTower(ref m_TowerDict);
    }

    void InitBullet()
    {
        Tools.AnalysisBullet(ref m_BulletDict);
    }


    /// <summary>
    /// 初始化单词字典
    /// </summary>
    void InitWordDict(string wordDir)
    {
        Tools.FillWords(ref m_WordDict, wordDir);
    }

    /// <summary>
    /// 初始化Persistent路径下的单词Json文件
    /// </summary>
    void InitPersistentWordFiles()
    {
        Tools.SaveWordInfoToPersistent(Consts.WordInitDir + Consts.WordJson_PrimaryDir, Consts.SaveDir + Consts.WordJson_PrimaryDir);
        Tools.SaveWordInfoToPersistent(Consts.WordInitDir + Consts.WordJson_CET4Dir, Consts.SaveDir + Consts.WordJson_CET4Dir);
        Tools.SaveWordInfoToPersistent(Consts.WordInitDir + Consts.WordJson_CET6Dir, Consts.SaveDir + Consts.WordJson_CET6Dir);
        Tools.SaveWordInfoToPersistent(Consts.WordInitDir + Consts.WordJson_PostgraduateDir, Consts.SaveDir + Consts.WordJson_PostgraduateDir);
    }

    /// <summary>
    /// 清空单词字典
    /// </summary>
    public void ClearWordDict()
    {
        m_WordDict.Clear();
    }

    /// <summary>
    /// 根据单词ID获取单词
    /// </summary>
    public Word GetWord(string wordId)
    {
        return m_WordDict[wordId];
    }

    /// <summary>
    /// 修改单词的正确或错误标记数
    /// </summary>
    public static void ModityWordMarkCount(string wordId,bool isRight)
    {
        if (isRight == true)
        {
            m_WordDict[wordId].RightMarkCount += 1;
        }
        else
        {
            m_WordDict[wordId].WrongMarkCount += 1;
        }
    }

    /// <summary>
    /// 根据怪物类型获取怪物数据
    /// </summary>
    public MonsterInfo GetMonsterInfo(int MonsterTypeID)
    {
        return m_MonsterDict[MonsterTypeID];
    }

    /// <summary>
    /// 获取宠物数据
    /// </summary>
    /// <param name="petId">宠物id</param>
    public PetInfo GetPetInfo(int petId)
    {
        return m_PetsDict[petId];
    }

    /// <summary>
    /// 获取塔的数据
    /// </summary>
    public TowerInfo GetTowerInfo(int towerID)
    {
        return m_TowerDict[towerID];
    }

    /// <summary>
    /// 获取子弹的数据
    /// </summary>
    public BulletInfo GetBulletInfo(int bulletID)
    {
        return m_BulletDict[bulletID];
    }
}

