using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor
{
    [HideInInspector]
    public Map Map = null;
    //关卡文件列表
    private List<FileInfo> m_files = new List<FileInfo>();

    //单词id集合
    public List<string> wordIdList = new List<string>();

    //选择关卡文件索引
    private int m_selectIndex = -1;
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying)
        {
            Map = target as Map;
            //主题索引从0开始
                
            EditorGUILayout.BeginHorizontal();
            int currentIndex = EditorGUILayout.Popup(m_selectIndex, GetFileName(m_files));
            if (currentIndex != m_selectIndex)
            {
                m_selectIndex = currentIndex;
                //加载关卡信息
                LoadLevel();
            }
            if (GUILayout.Button("读取列表"))
            {
                //读取关卡列表,使用map类的公用变量进行分主题读取，目的是避免每次编辑都要从90个文件中寻找
                LoadLevelFile(Map.themeIndex);
            }
            
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("清除塔点"))
            {
                Map.ClearHolder();
            }
            if (GUILayout.Button("清除路径"))
            {
                Map.ClearRoad();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("保存数据"))
            {
                //保存关卡信息
                SaveLevel();
            }

            //EditorGUILayout.EndHorizontal();

            //EditorGUILayout.BeginHorizontal();
            //int wordCount = 0;
            //int wordId = 0;
            //wordCount = EditorGUILayout.IntField("该关卡的单词数", wordCount);
            //Debug.Log("单词数：" + wordCount);
            //if (GUILayout.Button("确认"))
            //{

            //    int count = wordCount;
                
            //    EditorGUILayout.BeginHorizontal();
            //    EditorGUILayout.LabelField("请输入 " + count + " 个对应的单词ID");
            //    EditorGUILayout.EndHorizontal();

            //    for (int i = 1; i < count + 1; i++)
            //    {
            //        EditorGUILayout.BeginHorizontal();
            //        wordId = EditorGUILayout.IntField("单词" + i, wordId);
            //        if (GUILayout.Button("确认"))
            //            wordIdList.Add(wordId);
            //        EditorGUILayout.EndHorizontal();                   
            //    }
                
            //}      

            //EditorGUILayout.EndHorizontal();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

    }

    /// <summary>
    /// 加载关卡信息文件并设置显示关卡
    /// </summary>
    void LoadLevelFile(int themeIndex)
    {
        //清空状态
        Clear();

        //m_files = Tools.GetLevelFiles();
        m_files = Tools.GetLevelFilesOfTheme(themeIndex);
        if (m_files.Count > 0)
        {
            m_selectIndex = 0;
            LoadLevel();
        }
    }

    /// <summary>
    /// 加载关卡
    /// </summary>
    void LoadLevel()
    {
        //根据索引获得文件集合中对应的文件
        FileInfo file = m_files[m_selectIndex];

        Level level = new Level();
        //将文件数据填充入关卡信息类中
        Tools.FillLevel(file.Name, ref level);
        //地图通过关卡信息加载关卡
        Map.LoadLevel(level);
        wordIdList = level.WordIds;
    }

    /// <summary>
    /// 保存关卡
    /// </summary>
    void SaveLevel()
    {
        //当前地图关卡信息
        Level level = Map.Level;

        //临时索引点集合
        List<Point> list = null;

        list = new List<Point>();
        //获取此时所有可放置塔点坐标
        for (int i = 0; i < Map.Grid.Count; i++)
        {
            //获得该地图上所有格子
            Tile t = Map.Grid[i];
            if (t.CanHold == true)
            {
                Point p = new Point(t.X, t.Y);
                list.Add(p);
            }
        }
        level.Holder = list;

        list = new List<Point>();
        //获取此时地图上的敌人路径坐标集合
        for (int i = 0; i < Map.Road.Count; i++)
        {
            Tile t = Map.Road[i];
            Point p = new Point(t.X, t.Y);
            list.Add(p);
        }
        level.Path = list;

        //保存的文件路径
        string fileName = Application.dataPath + @"/Game/Resources/Levels/" + m_files[m_selectIndex].Name + ".xml";
        Debug.Log(fileName);

        //保存关卡信息
        Tools.SaveLevel(fileName, level);

        //保存成功提示框
        EditorUtility.DisplayDialog("保存提示", "保存成功", "确认");
    }

    /// <summary>
    /// 清空信息
    /// </summary>
    void Clear()
    {
        m_files.Clear();
        m_selectIndex = -1;
    }

    /// <summary>
    /// 获取文件列表的对应字符串名称
    /// </summary>
    private string[] GetFileName(List<FileInfo> files)
    {
        List<string> fileNames = new List<string>();
        foreach (FileInfo f in files)
        {
            fileNames.Add(f.Name);
        }
        //for (int i = themeIndex * 15; i <= (themeIndex * 15) + 15; i++)
        //{
        //    fileNames.Add(files[i].Name);
        //}
        return fileNames.ToArray();
    }
}
