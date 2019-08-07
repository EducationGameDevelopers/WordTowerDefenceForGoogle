using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPanel:MonoBehaviour
{
    //塔的图标
    private TowerIcon[] m_TowerIcons;

    private CircleShaderController csc;

    void Awake()
    {
        m_TowerIcons = transform.GetComponentsInChildren<TowerIcon>();
    }

    /// <summary>
    /// 显示选择塔面板
    /// </summary>
    /// <param name="postion"></param>
    public void Show(GameModel gm, Vector3 position, bool upSide)
    {
        List<string> towerIds = gm.LevelList[gm.PlayProgress].TowerIds;
        //动态加载塔的图标
        for (int i = 0; i < m_TowerIcons.Length; i++)
        {
            //获取对应塔的信息
            TowerInfo info = Game.Instance.a_StaticData.GetTowerInfo(int.Parse(towerIds[i]));
            //加载塔的图标
            m_TowerIcons[i].LoadIcon(gm, info, position, upSide);
        }

        //设置显示位置
        Vector2 spawnPanelPos = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
        transform.position = spawnPanelPos;
        gameObject.SetActive(true);
        if (Game.Instance.isFirst)
        {
            csc = transform.parent.parent.Find("BG_Dark").GetComponent<CircleShaderController>();
            if (csc.isGuideCallTower)
            {
                Image towerIcon = transform.Find("TowerIcon1").GetComponent<Image>();
                csc.Targets[4] = towerIcon;
                csc.remindString = RemindString.callTower;
                csc.ChangeTarget();
            }
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

