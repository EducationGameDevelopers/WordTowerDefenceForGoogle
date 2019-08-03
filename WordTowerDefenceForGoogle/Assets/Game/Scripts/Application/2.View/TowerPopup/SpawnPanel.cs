using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SpawnPanel:MonoBehaviour
{
    //塔的图标
    private TowerIcon[] m_TowerIcons;

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
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

