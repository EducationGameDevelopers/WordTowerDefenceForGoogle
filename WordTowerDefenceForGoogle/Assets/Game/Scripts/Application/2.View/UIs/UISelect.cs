using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class UISelect : View
{
    private GameModel m_GameModel = null;
    private List<Card> m_Cards = null;
    private GameObject map;
    private GameObject valcano;
    private GameObject bog;
    private GameObject forest;
    private GameObject snowMountain;
    private GameObject night;
    private GameObject desert;
    private GameObject magma;
    private List<Transform> themeMaps;      //存储各个大地图的列表
    private List<Transform> maps;  //存储各个主题的小地图的列表

    public override string Name
    {
        get { return Consts.V_UISelect; }
    }
    #region Unity回调
    private void Awake()
    {
        map = GameObject.Find("Map");
        valcano = GameObject.Find("Valcano");
        bog = GameObject.Find("Bog");
        forest = GameObject.Find("Forest");
        snowMountain = GameObject.Find("SnowMountain");
        night = GameObject.Find("Night");
        desert = GameObject.Find("Desert");
        magma = GameObject.Find("Magma");
        themeMaps = new List<Transform>();
        maps = new List<Transform>();
        foreach(Transform m in map.GetComponentsInChildren<Transform>(true))
        {
            themeMaps.Add(m);        //maps包括自身和孩子节点
        }
        maps.Add(valcano.transform);
        maps.Add(bog.transform);
        maps.Add(forest.transform);
        maps.Add(snowMountain.transform);
        maps.Add(night.transform);
        maps.Add(desert.transform);
        maps.Add(magma.transform);
    }
    private void Start()
    {
        InitMap();
        foreach(Transform map in maps)
        {
            map.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 火山主题按钮点击相应事件，显示火山小地图
    /// </summary>
    public void OnValcanoClick()
    {
        valcano.SetActive(true);
    }

    public void OnBackClick()
    {
        Game.Instance.LoadScene(1);
    }
    #endregion
    #region 方法
    ///// <summary>
    ///// 初始化地图
    ///// </summary>
    ///// <param name="index1"></param>
    ///// <param name="index2"></param>
    //private void InitMap()
    //{
    //    string Dirpath = Consts.SaveDir;
    //    string fileName = "MapInformation.xml";
    //    int index1 = 0;
    //    int index2 = 0;
    //    try
    //    {

    //        XmlDocument xmlDoc = new XmlDocument();
    //        xmlDoc.Load(Dirpath + fileName);
    //        XmlNode root = xmlDoc.DocumentElement;
    //        for (index1 = 0; index1 < root.ChildNodes.Count; index1++)
    //        {
    //            XmlNode themeNode = root.ChildNodes.Item(index1);
    //            if (index1 == root.ChildNodes.Count - 1)
    //            {
    //                //第七个主题
    //                if(bool.Parse(themeNode.ChildNodes.Item(0).InnerText))
    //                {
    //                    themeMaps[index1+1].gameObject.SetActive(true);
    //                    maps[index1].gameObject.SetActive(true);
    //                }
    //                else
    //                {
    //                    themeMaps[index1+1].gameObject.SetActive(false);
    //                    maps[index1].gameObject.SetActive(false);
    //                }
    //            }
    //            else
    //            {
    //                index2 = 0;
    //                foreach (Transform levelTransform in maps[index1].transform)
    //                {
    //                    if (index2 == 15)
    //                        break;
    //                    if (index2 == 0)
    //                    {
    //                        //每个主题的IsLight信息
    //                        XmlNode isLightNode = themeNode.ChildNodes.Item(0);
    //                        if (bool.Parse(isLightNode.InnerText))
    //                        {
    //                            //该主题开启
    //                            //Console.WriteLine(string.Format("第{0}个大地图开启", index1));
    //                            themeMaps[index1+1].gameObject.SetActive(true);
    //                            maps[index1].gameObject.SetActive(true);
    //                        }
    //                        else
    //                        {
    //                            themeMaps[index1+1].gameObject.SetActive(false);
    //                            maps[index1].gameObject.SetActive(false);
    //                        }
    //                        index2++;
    //                        //由于xml中，Theme下面有16个子节点，而每个map只有15个地图，只能循环15次，因此在第一次循环的时候将第一个小地图的信息读取出来
    //                        XmlNode levelNode = themeNode.ChildNodes.Item(1);
    //                        XmlNode levelIslightNode = levelNode.ChildNodes.Item(0);
    //                        if (bool.Parse(levelIslightNode.InnerText))     //该关卡可以点击
    //                        {
    //                            int i = 0;  //用于下面循环的计数
    //                            levelTransform.gameObject.GetComponent<Button>().interactable = true;
    //                            foreach (Transform imageTransform in levelTransform)
    //                            {
    //                                if (i == 0)
    //                                {
    //                                    //将关卡图片设置为可点击
    //                                    imageTransform.gameObject.SetActive(true);
    //                                }
    //                                else
    //                                {
    //                                    imageTransform.gameObject.SetActive(false);
    //                                }
    //                                i++;
    //                            }
    //                        }
    //                        else
    //                        {
    //                            int i = 0;  //用于下面循环的计数
    //                            levelTransform.gameObject.GetComponent<Button>().interactable = false;
    //                            foreach (Transform imageTransform in levelTransform)
    //                            {
    //                                if (i == 0)
    //                                {
    //                                    //将关卡图片设置为不可点击
    //                                    imageTransform.gameObject.SetActive(false);
    //                                }
    //                                else
    //                                {
    //                                    imageTransform.gameObject.SetActive(true);
    //                                }
    //                                i++;
    //                            }
    //                        }
    //                    }
    //                    else    //index2从0开始
    //                    {
    //                        //每个小地图的信息
    //                        XmlNode levelNode = themeNode.ChildNodes.Item(index2);
    //                        XmlNode levelIslightNode = levelNode.ChildNodes.Item(0);
    //                        if (bool.Parse(levelIslightNode.InnerText))     //该关卡可以点击
    //                        {
    //                            int i = 0;  //用于下面循环的计数
    //                            levelTransform.gameObject.GetComponent<Button>().interactable = true;
    //                            foreach (Transform imageTransform in levelTransform)
    //                            {
    //                                if (i == 0)
    //                                {
    //                                    //将关卡图片设置为可点击
    //                                    imageTransform.gameObject.SetActive(true);
    //                                }
    //                                else
    //                                {
    //                                    imageTransform.gameObject.SetActive(false);
    //                                }
    //                                i++;
    //                            }
    //                        }
    //                        else
    //                        {
    //                            int i = 0;  //用于下面循环的计数
    //                            levelTransform.gameObject.GetComponent<Button>().interactable = false;
    //                            foreach (Transform imageTransform in levelTransform)
    //                            {
    //                                if (i == 0)
    //                                {
    //                                    //将关卡图片设置为不可点击
    //                                    imageTransform.gameObject.SetActive(false);
    //                                }
    //                                else
    //                                {
    //                                    imageTransform.gameObject.SetActive(true);
    //                                }
    //                                i++;
    //                            }
    //                        }
    //                    }
    //                    index2++;
    //                }
    //            }

    //        }
    //    }catch (FileNotFoundException e)
    //    {
    //        Debug.Log("地图信息文件未找到");
    //    }
    //    catch(NullReferenceException e)
    //    {
    //        Debug.Log("在" + index1 + "个大地图中的第" + index2);
    //    }
    //}

    private void InitMap()
    {
        int[] indexArray = Tools.TransformLevelIndex(m_GameModel.MaxPassProgress);
        int themeIndex = indexArray[0];
        int levelIndex = indexArray[1];
        for(int i = 0; i <= themeIndex; i++)    //将第一个到玩家最大进度的主题关卡点亮
        {
            themeMaps[i+1].gameObject.SetActive(true);
            if(i < themeIndex)      //不是最大进度的主题关卡，将其中的小地图全部点亮
            {
                foreach(Transform levelTransform in maps[i])
                {
                    levelTransform.gameObject.SetActive(true);
                    SetLevel(levelTransform, true);
                }
            }
            else
            {
                if(i == 6)  //如果是第6个地图
                {
                    maps[i].gameObject.SetActive(true);
                }
                else     //不是第六个主题，将其中从第一个到第levelIndex-1个小地图点亮,并将后面的小地图设置为未点亮
                {
                    int j = 0;
                    foreach(Transform levelTransform in maps[i])
                    {
                        if(j == 15)
                        {
                            levelTransform.gameObject.SetActive(true);
                            break;
                        }
                        levelTransform.gameObject.SetActive(true);
                        if(j <= levelIndex)
                        {
                            SetLevel(levelTransform, true);
                        }
                        else
                        {
                            SetLevel(levelTransform, false);
                        }
                        j++;
                    }
                }
            }
        }
        for(int i = themeIndex+1; i <= 6; i++)   //将剩下的主题设置为未点亮
        {
            themeMaps[i+1].gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 设置该小地图是否可以点击
    /// </summary>
    /// <param name="levelTransform"></param>
    /// <param name="isAble"></param>
    private void SetLevel(Transform levelTransform, bool isAble)
    {
        int j = 0;  //用于下面循环的计数
        levelTransform.gameObject.GetComponent<Button>().interactable = isAble;
        foreach (Transform imageTransform in levelTransform)
        {
            if (j == 0)
            {
                //将关卡图片设置为可点击
                imageTransform.gameObject.SetActive(isAble);
            }
            else
            {
                imageTransform.gameObject.SetActive(!isAble);
            }
            j++;
        }
    }
    /// <summary>
    /// 选择关卡
    /// </summary>
    public void ChooseLevel(int levelIndex)
    {
        //开始关卡事件发送
        StartLevelArgs e = new StartLevelArgs()
        {
            LevelIndex = levelIndex
        };

        SendEvent(Consts.E_StartLevel, e);
    }
    #endregion



    #region 事件回调
    public override void HandleEvent(string eventName, object data)
    {
        switch(eventName)
        {
            case Consts.E_EnterScene:
                SceneArgs e = data as SceneArgs;
                if(e.SceneIndex == 2)
                {
                    //选择关卡场景
                    m_GameModel = GetModel<GameModel>();
                }
                break;
            default: break;
        }
    }
    public override void RegisterAttentionEvent()
    {
        //关心进入场景事件
        this.AttentionEventList.Add(Consts.E_EnterScene);
    }

    #endregion

}
