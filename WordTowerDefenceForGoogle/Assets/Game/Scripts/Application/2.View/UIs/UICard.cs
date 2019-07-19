using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICard : MonoBehaviour
{
    public event Action<Card> OnClick;  //委托事件，鼠标点击卡片

    public Image ImgCard;
    public Image ImgLock;

    private Card m_Card = null;

    private bool isTransparent = false;   //卡片是否半透明

    /// <summary>
    /// 卡片半透明设置
    /// </summary>
    public bool IsTransparent
    {
        get { return isTransparent; }
        set
        {
            isTransparent = value;

            Image[] images = new Image[] { ImgCard, ImgLock };
            foreach (Image img in images)
            {
                //获取当前卡片色彩属性
                Color color = img.color;
                color.a = value ? 0.5f : 1;
                img.color = color;
            }
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void DataBind(Card card)
    {
        m_Card = card;
        string[] imageName = m_Card.CardImage.Split('.');
        string cardFile = "Cards/" + imageName[0];

        //根据路径加载图片资源并显示
        StartCoroutine(Tools.LoadImage(cardFile, ImgCard));
        //是否有锁
        isTransparent = m_Card.IsLocked;
        //锁的图标是否显示
        ImgLock.gameObject.SetActive(m_Card.IsLocked);
    }


   

    public void OnAddClick()
    {
        if (OnClick != null)
        {
            OnClick(m_Card);
        }
    }

    void OnDestory()
    {
        //当该游戏物体销毁时，清空委托事件
        while (OnClick != null)
        {
            OnClick -= OnClick;
        }
    }
}

