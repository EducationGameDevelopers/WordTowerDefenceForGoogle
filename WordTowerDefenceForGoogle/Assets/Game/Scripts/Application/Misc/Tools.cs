using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Xml;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using LitJson;
using System.Text.RegularExpressions;    //正则表达式处理

public class Tools
{
    public static JsonData CurrentWordJsonData;    //当前单词Json类
    public static string CurrentSaveJsonDir;      //当前存储Json文件的路径

    /// <summary>
    /// 将纯粹的关卡数字索引转换成主题加小地图索引,以数组的形式返回，数组的第一个是主题索引，第二个是小地图索引
    /// </summary>
    /// <param name="index">从0开始</param>
    /// <returns></returns>
    public static int[] TransformLevelIndex(int index)
    {
        int themeIndex = index / 15;
        //levelIndex表示最后一个主题的小地图的进度
        int levelIndex = index % 15;
        int[] indexArray = new int[2];
        indexArray[0] = themeIndex;
        indexArray[1] = levelIndex;
        return indexArray;
    }
    /// <summary>
    /// 获得玩家的最大进度数据
    /// </summary>
    /// <returns></returns>
    public static int GetMaxProgress()
    {
        XmlDocument doc = new XmlDocument();

        TextAsset myText = Resources.Load(Consts.SaveDir + "mapInformation.xml") as TextAsset;
        XmlReader reader = XmlReader.Create(new StringReader(myText.text));
        doc.Load(reader);
        return int.Parse(doc.SelectSingleNode("Progress").InnerText);
    }
    /// <summary>
    /// 获取关卡文件集合
    /// </summary>
    public static List<FileInfo> GetLevelFiles()
    {
        TextAsset[] xmlTexts = Resources.LoadAll<TextAsset>("Levels");
      
        List<FileInfo> list = new List<FileInfo>();
        //将文件名称包装成文件加入文件集合中
        for (int i = 0; i < xmlTexts.Length; i++)
        {
            FileInfo file = new FileInfo(xmlTexts[i].name);
            list.Add(file);         
        }
        
        return list;
    }
    /// <summary>
    /// 获取某一主题文件集合
    /// </summary>
    public static List<FileInfo> GetLevelFilesOfTheme(int themeIndex)
    {
        TextAsset[] xmlTexts = Resources.LoadAll<TextAsset>("Levels");

        List<FileInfo> list = new List<FileInfo>();
        //将文件名称包装成文件加入文件集合中
        for (int i = themeIndex*15; i < (themeIndex*15)+15; i++)
        {
            FileInfo file = new FileInfo(xmlTexts[i].name);
            list.Add(file);
        }

        return list;
    }
    /// <summary>
    /// 填充Level类中的数据(从Xml文件中获取数据并填充)
    /// </summary>
    public static void FillLevel(string fileName, ref Level level)
    {
        XmlDocument doc = new XmlDocument();

        TextAsset myText = Resources.Load("Levels" + "/" + fileName) as TextAsset;
        XmlReader reader = XmlReader.Create(new StringReader(myText.text));
        doc.Load(reader);
        //单个结点写入XML文件
        level.Name = doc.SelectSingleNode("/Level/Name").InnerText;
        level.CardImage = doc.SelectSingleNode("/Level/CardImage").InnerText;
        level.Background = doc.SelectSingleNode("/Level/Background").InnerText;
        level.Road = doc.SelectSingleNode("/Level/Road").InnerText;
        level.InitScore = int.Parse(doc.SelectSingleNode("/Level/InitScore").InnerText);

        XmlNodeList nodes;

        //放置炮塔位置坐标点的解析与填充
        nodes = doc.SelectNodes("/Level/Holder/Point");
        for (int i = 0; i < nodes.Count; i++)
        {
            XmlNode node = nodes[i];
            //读取该子节点的坐标属性并实例化坐标点类
            Point p = new Point(int.Parse(node.Attributes["X"].Value), int.Parse(node.Attributes["Y"].Value));

            level.Holder.Add(p);
        }

        //敌人行进路径坐标点的解析与填充
        nodes = doc.SelectNodes("/Level/Path/Point");
        for (int i = 0; i < nodes.Count; i++)
        {
            XmlNode node = nodes[i];
            //读取该子节点的坐标属性并实例化坐标点类
            Point p = new Point(int.Parse(node.Attributes["X"].Value), int.Parse(node.Attributes["Y"].Value));

            level.Path.Add(p);
        }

        //当前关卡的敌人信息的解析与填充
        nodes = doc.SelectNodes("/Level/Rounds/Round");
        for (int i = 0; i < nodes.Count; i++)
        {
            XmlNode node = nodes[i];
            //读取该结点的怪物信息属性并实例化
            Round r = new Round(int.Parse(node.Attributes["Monster"].Value), int.Parse(node.Attributes["Count"].Value));

            level.Rounds.Add(r);
        }

        //当前关卡的单词解析与填充
        nodes = doc.SelectNodes("/Level/WordIds/WordId");
        for (int i = 0; i < nodes.Count; i++)
        {
            XmlNode node = nodes[i];
            string wordId = node.Attributes["Id"].Value;
            level.WordIds.Add(wordId);
        }

        nodes = doc.SelectNodes("/Level/TowerIds/Tower");
        for(int i = 0; i < nodes.Count; i++)
        {
            XmlNode node = nodes[i];
            string towerId = node.Attributes["Id"].Value;
            level.TowerIds.Add(towerId);
        }
        
    }

    /// <summary>
    /// 读取Monster.xml文件获取怪物信息
    /// </summary>
    /// <param name="m_MonsterDict"></param>
    public static void AnalysisMonster(ref Dictionary<int, MonsterInfo> m_MonsterDict)
    {
        XmlDocument doc = new XmlDocument();

        TextAsset textAsset = Resources.Load("GameStaticData/Monster") as TextAsset;
        XmlReader reader = XmlReader.Create(new StringReader(textAsset.text));

        doc.Load(reader);

        //遍历根节点
        XmlNode root = doc.DocumentElement;
        foreach(XmlNode node in root.ChildNodes)
        {
            if(node.Name.Equals("monster"))
            {
                //获取所有monster子节点的信息存入MonsterInfo，然后存入字典。
                MonsterInfo monster = new MonsterInfo();
                foreach(XmlNode subnode in node)
                {
                    switch(subnode.Name)
                    {
                        case "ID":
                            monster.ID = int.Parse(subnode.InnerText);
                            break;
                        case "HP":
                            monster.Hp = int.Parse(subnode.InnerText);
                            break;
                        case "MoveSpeed":
                            monster.MoveSpeed = float.Parse(subnode.InnerText);
                            break;
                        case "RewardMoney":
                            monster.RewardMoney = int.Parse(subnode.InnerText);
                            break;
                        case "Damage":
                            monster.Damage = int.Parse(subnode.InnerText);
                            break;
                    }
                }
                m_MonsterDict.Add(monster.ID, monster);
            }
        }
    }

    /// <summary>
    /// 读取Tower.xml文件获取塔信息
    /// </summary>
    /// <param name="m_TowerDict"></param>
    public static void AnalysisTower(ref Dictionary<int, TowerInfo> m_TowerDict)
    {
        XmlDocument doc = new XmlDocument();

        TextAsset textAsset = Resources.Load("GameStaticData/Tower") as TextAsset;
        XmlReader reader = XmlReader.Create(new StringReader(textAsset.text));

        doc.Load(reader);

        //遍历根节点
        XmlNode root = doc.DocumentElement;
        foreach (XmlNode node in root.ChildNodes)
        {
            if (node.Name.Equals("tower"))
            {
                //获取所有tower子节点的信息存入TowerInfo，然后存入字典。
                TowerInfo tower = new TowerInfo();
                foreach (XmlNode subnode in node)
                {
                    switch (subnode.Name)
                    {
                        case "TowerID":
                            tower.TowerID = int.Parse(subnode.InnerText);
                            break;
                        case "TowerPrefabName":
                            tower.TowerPrefabName = subnode.InnerText;
                            break;
                        case "EnableIcon":
                            tower.EnableIcon = subnode.InnerText;
                            break;
                        case "DisableIcon":
                            tower.DisableIcon = subnode.InnerText;
                            break;
                        case "BasePrice":
                            tower.BasePrice = int.Parse(subnode.InnerText);
                            break;
                        case "AttackRange":
                            tower.AttackRange = float.Parse(subnode.InnerText);
                            break;
                        case "MaxLevel":
                            tower.MaxLevel = int.Parse(subnode.InnerText);
                            break;
                        case "ShotRate":
                            tower.ShotRate = float.Parse(subnode.InnerText);
                            break;
                        case "UseBulletID":
                            tower.UseBulletID = int.Parse(subnode.InnerText);
                            break;
                    }
                }
                m_TowerDict.Add(tower.TowerID, tower);
            }
        }
    }

    /// <summary>
    /// 读取Bullet.xml文件获取怪物信息
    /// </summary>
    /// <param name="m_BulletDict"></param>
    public static void AnalysisBullet(ref Dictionary<int, BulletInfo> m_BulletDict)
    {
        XmlDocument doc = new XmlDocument();

        TextAsset textAsset = Resources.Load("GameStaticData/Bullet") as TextAsset;
        XmlReader reader = XmlReader.Create(new StringReader(textAsset.text));

        doc.Load(reader);

        //遍历根节点
        XmlNode root = doc.DocumentElement;
        foreach (XmlNode node in root.ChildNodes)
        {
            if (node.Name.Equals("bullet"))
            {
                //获取所有bullet子节点的信息存入BulletInfo，然后存入字典。
                BulletInfo bullet = new BulletInfo();
                foreach (XmlNode subnode in node)
                {
                    switch (subnode.Name)
                    {
                        case "BulletID":
                            bullet.BulletID = int.Parse(subnode.InnerText);
                            break;
                        case "PrefabName":
                            bullet.PrefabName = subnode.InnerText;
                            break;
                        case "BaseSpeed":
                            bullet.BaseSpeed = float.Parse(subnode.InnerText);
                            break;
                        case "BaseDamage":
                            bullet.BaseDamage = int.Parse(subnode.InnerText);
                            break;
                        case "IsCanTough":
                            bullet.IsCanTough = bool.Parse(subnode.InnerText);
                            break;
                    }
                }
                m_BulletDict.Add(bullet.BulletID, bullet);
            }
        }
    }

    /// <summary>
    /// 读取
    /// pet.xml文件获取怪物信息
    /// </summary>
    /// <param name="m_PetsDict"></param>
    public static void AnalysisPet(ref Dictionary<int, PetInfo> m_PetsDict)
    {
        XmlDocument doc = new XmlDocument();

        TextAsset textAsset = Resources.Load("GameStaticData/Pet") as TextAsset;
        XmlReader reader = XmlReader.Create(new StringReader(textAsset.text));

        doc.Load(reader);

        //遍历根节点
        XmlNode root = doc.DocumentElement;
        foreach (XmlNode node in root.ChildNodes)
        {
            if (node.Name.Equals("pet"))
            {
                //获取所有pet子节点的信息存入PetInfo，然后存入字典。
                PetInfo pet = new PetInfo();
                foreach (XmlNode subnode in node)
                {
                    switch (subnode.Name)
                    {
                        case "ID":
                            pet.ID = int.Parse(subnode.InnerText);
                            break;
                        case "HP":
                            pet.Hp = int.Parse(subnode.InnerText);
                            break;
                    }
                }
                m_PetsDict.Add(pet.ID, pet);
            }
        }
    }

    /// <summary>
    /// 保存关卡信息(写入到Xml文件中)
    /// </summary>
    public static void SaveLevel(string fileName, Level level)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        //根节点写入
        sb.AppendLine("<Level>");

        //写入单个结点
        sb.AppendLine(string.Format("<Name>{0}</Name>", level.Name));
        sb.AppendLine(string.Format("<CardImage>{0}</CardImage>", level.CardImage));
        sb.AppendLine(string.Format("<Background>{0}</Background>", level.Background));
        sb.AppendLine(string.Format("<Road>{0}</Road>", level.Road));
        sb.AppendLine(string.Format("<InitScore>{0}</InitScore>", level.InitScore));

        //写入放置炮塔位置坐标点
        sb.AppendLine("<Holder>");
        for (int i = 0; i < level.Holder.Count; i++)
        {
            sb.AppendLine(string.Format("<Point X=\"{0}\" Y=\"{1}\"/>", level.Holder[i].X, level.Holder[i].Y));
        }
        sb.AppendLine("</Holder>");

        //写入敌人行进路径坐标点
        sb.AppendLine("<Path>");
        for (int i = 0; i < level.Path.Count; i++)
        {
            sb.AppendLine(string.Format("<Point X=\"{0}\" Y=\"{1}\"/>", level.Path[i].X, level.Path[i].Y));
        }
        sb.AppendLine("</Path>");

        //写入当前关卡敌人信息
        sb.AppendLine("<Rounds>");
        for (int i = 0; i < level.Rounds.Count; i++)
        {
            sb.AppendLine(string.Format("<Round Monster=\"{0}\" Count=\"{1}\"/>", level.Rounds[i].MonsterID, level.Rounds[i].Count));
        }
        sb.AppendLine("</Rounds>");

        //当前关卡中的单词
        sb.AppendLine("<WordIds>");
        for (int i = 0; i < level.WordIds.Count; i++)
        {
            sb.AppendLine(string.Format("<WordId Id=\"{0}\"/>", level.WordIds[i]));
        }
        sb.AppendLine("</WordIds>");

        sb.AppendLine("</Level>");

        string content = sb.ToString();
        //Xml文件写入操作设置
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;   //缩进
        settings.ConformanceLevel = ConformanceLevel.Auto;
        settings.IndentChars = "\t";  //缩进距离
        settings.OmitXmlDeclaration = false;  //不编写Xml声明

        //创建Xml写入流
        XmlWriter xw = XmlWriter.Create(fileName, settings);

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(content);
        doc.WriteTo(xw);

        xw.Flush();   //重写时刷新
        xw.Close();   //写入流关闭
    }

    /// <summary>
    /// 将Resources文件下的单词Json文件信息拷贝到persistentDataPath路径下
    /// </summary>
    /// <param name="wordResDir">原Resources文件夹下单词Json</param>
    /// <param name="wordPerDir">存入的PersistentPath路径</param>
    public static void SaveWordInfoToPersistent(string wordResDir, string wordPerDir)
    {
        //加载Json文件
        TextAsset wordJsonText = Resources.Load<TextAsset>(wordResDir);

        //将Resource文件下的Json转存到persistentDataPath路径下
        JsonData jD = JsonMapper.ToObject(wordJsonText.ToString());
        if (File.Exists(wordPerDir) == false)
        {
            SaveWordToJson(jD, wordPerDir);
        }       
    }

    /// <summary>
    /// 填充Word类中中数据
    /// </summary>
    public static void FillWords(ref Dictionary<string, Word> wordDict, string wordDir)
    {
        //创建文件流
        FileStream fs = new FileStream(wordDir, FileMode.Open);
       
        StreamReader sr = new StreamReader(fs);
      
        JsonData jd = JsonMapper.ToObject(sr.ReadToEnd());
        string str = jd.ToJson();
       
        //json存入后会被转义成编码，使用正则显示中文
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        string wordJson = reg.Replace(str, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });

        CurrentWordJsonData = jd;      //选定当前JsonData
        CurrentSaveJsonDir = wordDir;   //选定当前存储路径

        JSONObject jo = new JSONObject(wordJson);
        //遍历解析Json中的信息
        foreach (var temp in jo.list)
        {
            Word word = null;
            string str_Id = temp["Id"].str.ToString().PadLeft(4, '0');    //单词id
            string str_English = temp["English"].str;
            string str_Chinese = temp["Chinese"].str;
            int wrongMarkCount = (int)(temp["WrongMarkCount"].n);
            int rightMarkCount = (int)(temp["RightMarkCount"].n);

            word = new Word(str_Id, str_English, str_Chinese, wrongMarkCount, rightMarkCount);

            wordDict.Add(word.Str_Id, word);
        }

        sr.Close();
        fs.Close();      
    }

    /// <summary>
    /// 将单词信息存入Json文件
    /// </summary>
    public static void SaveWordToJson(JsonData jsonData, string wordInfoDir)
    {
        string str = jsonData.ToJson();
        //json存入后会被转义成编码，使用正则显示中文
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        string modifyString = reg.Replace(str, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });

        string pathDir = wordInfoDir;
        FileStream file = null;
        //当该文件路径不存在时
        if (!File.Exists(pathDir))
        {
            //创建该路径文件
             file = File.Create(pathDir);
        }
        else
        {
            //文件流写入UTF8格式内容
            file = new FileStream(pathDir, FileMode.Open);
        }
        byte[] bts = System.Text.Encoding.UTF8.GetBytes(modifyString);

        file.Write(bts, 0, bts.Length);

        //关闭文件流
        file.Close();
        
    }

/*
    /// <summary>
    /// 修改Json文件中的数据
    /// </summary>
    /// <param name="jsonData"></param>
    /// <param name="word_id"></param>
    public static void ModifyWordJson(string word_id, int wrongMarkCount, int rightMarkCount)
    {
        for (int i = 0; i < CurrentWordJsonData.Count; i++)
        {
            if (CurrentWordJsonData[i]["id"].Equals(word_id))
            {
                CurrentWordJsonData[i]["WrongMarkCount"] = wrongMarkCount;
                CurrentWordJsonData[i]["RightMarkCount"] = rightMarkCount;
            }
        }
    }

    /// <summary>
    /// 将单词类列表存入json
    /// </summary>
    public static void SaveModifyWordInfo(List<Word> wordList)
    {
        if (wordList != null)
        {
            foreach (Word word in wordList)
            {
                ModifyWordJson(word.Str_Id, word.WrongMarkCount, word.RightMarkCount);
            }
        }
        //将已修改完的JsonData存入相应路径下的Json文件
        SaveWordToJson(CurrentWordJsonData, CurrentSaveJsonDir);
    }
    */

    /// <summary>
    /// 将单词字典存入相应路径下的Json文件
    /// </summary>
    public static void SaveWordDicToJson(Dictionary<string, Word> wordDic)
    {
        JsonData jd = new JsonData();
        foreach (Word word in wordDic.Values)
        {
            JsonWriter jw = new JsonWriter();
            jw.WriteObjectStart();
            jw.WritePropertyName("Id");
            jw.Write(word.Str_Id);

            jw.WritePropertyName("English");
            jw.Write(word.Str_English);

            jw.WritePropertyName("Chinese");
            jw.Write(word.Str_Chinese);

            jw.WritePropertyName("WrongMarkCount");
            jw.Write(word.WrongMarkCount);

            jw.WritePropertyName("RightMarkCount");
            jw.Write(word.RightMarkCount);
            jw.WriteObjectEnd();

            jd.Add(JsonMapper.ToObject(jw.ToString()));
        }
        Debug.Log(CurrentSaveJsonDir);
        SaveWordToJson(jd, CurrentSaveJsonDir);
    }


    /// <summary>
    /// 加载图片
    /// </summary>
    /// <param name="url">图片的绝对地址</param>
    /// <param name="render">图片渲染组件</param>
    public static IEnumerator LoadImage(string url, SpriteRenderer render)
    {
        //使用Resources异步加载方式
        ResourceRequest request = Resources.LoadAsync(url, typeof(Texture2D));
        while (!request.isDone)
        {
            yield return request;
        }

        Texture2D texture = request.asset as Texture2D;

        //渲染图片，确定大小、位置
        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        render.sprite = sp;
    }


    public static IEnumerator LoadImage(string url, Image image)
    {
        ResourceRequest request = Resources.LoadAsync(url, typeof(Texture2D));
        while (!request.isDone)
        {
            yield return request;
        }

        Texture2D texture = request.asset as Texture2D;
        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        image.sprite = sp;
    }

}

