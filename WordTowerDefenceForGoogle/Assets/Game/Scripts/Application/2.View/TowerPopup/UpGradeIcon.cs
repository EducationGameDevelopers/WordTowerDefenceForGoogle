using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UpGradeIcon : MonoBehaviour
{

    private Tower m_Tower;

    private Text m_UpGradeValue;
    void Awake()
    {
        m_UpGradeValue = transform.Find("UpGradeValue").GetComponent<Text>();
    }

    /// <summary>
    /// 加载对应的图标
    /// </summary>
    public void LoadIcon(Tower tower)
    {
        m_Tower = tower;
      
        if(m_Tower.IsTopLevel==true)
        {
            m_UpGradeValue.text = "顶级";
        }
        else
        {
            if(m_Tower.Level== 1)
                m_UpGradeValue.text = m_Tower.BasePrice.ToString();
            m_UpGradeValue.text = m_Tower.Price.ToString();
        }
        
    }


    public void  OnUpGradeOnClick()
    {
        SendMessageUpwards("OnUpGradeTower", m_Tower, SendMessageOptions.RequireReceiver);
    }
    
}

