using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UpGradePanel:MonoBehaviour
{
    //升级图标类
    private UpGradeIcon m_UpGradeIcon;

    //出售图标类
    private SellIcon m_SellIcon;

    private Tower m_Tower;

    private CircleShaderController csc;
    void Awake()
    {
        m_UpGradeIcon = transform.GetComponentInChildren<UpGradeIcon>();
        m_SellIcon = transform.GetComponentInChildren<SellIcon>();
    }

    /// <summary>
    /// 升级塔界面显示
    /// </summary>
    public void Show(Tower tower)
    {
        gameObject.SetActive(true);
        if(Game.Instance.isFirst)
        {
            csc = GameObject.Find("BG_Dark").GetComponent<CircleShaderController>();
            csc.remindText.text = "";
            Image darkImage = csc.gameObject.GetComponent<Image>();
            darkImage.color = new Color(0, 0, 0, 0);
            darkImage.enabled = false;
            csc.isGuideCallTower = false;
        }
        Vector2 upGradePos = RectTransformUtility.WorldToScreenPoint(Camera.main, tower.transform.position);
        transform.position = upGradePos;
        m_Tower = tower;

        //对应图标加载
        m_UpGradeIcon.LoadIcon(m_Tower);
        m_SellIcon.LoadIcon(m_Tower);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

