using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SellIcon : MonoBehaviour
{
    private Tower m_Tower;

    private Text m_SellValue;

    private float sellValue;

    void Awake()
    {
        m_SellValue = transform.Find("SellValue").GetComponent<Text>();
    }


    /// <summary>
    /// 加载对应的图标
    /// </summary>
    public void LoadIcon(Tower tower)
    {
        m_Tower = tower;
        //出售价格设置
        sellValue = m_Tower.Price / 2;
        m_SellValue.text = sellValue.ToString();
    }
   
    public void OnSellClick()
    {
        //向上层发送消息，最终TowerPopup脚本接收该消息并处理
        SendMessageUpwards("OnSellTower", m_Tower, SendMessageOptions.RequireReceiver);
    }

    
}

