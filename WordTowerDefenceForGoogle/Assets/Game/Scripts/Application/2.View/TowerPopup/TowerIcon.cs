using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TowerIcon : MonoBehaviour
{
    private Image m_SpriteRenderer;

    private TowerInfo m_TowerInfo;  //该塔的信息
    private Vector3 m_Position;     //图标出现的位置

    private bool m_GoldIsEnough = false;   //金币是否足够

    private Button m_SpawnTowerBtn;

    private Text m_PriceText;   //价格显示文本

    void Awake()
    {
        m_SpriteRenderer = gameObject.GetComponent<Image>();
        m_SpawnTowerBtn = transform.GetComponent<Button>();
        m_SpawnTowerBtn.onClick.AddListener(OnTowerClick);
        m_PriceText = transform.Find("PriceBg/PriceLabel").GetComponent<Text>();
    }
    /// <summary>
    /// 加载塔的图片
    /// </summary>
    public void LoadIcon(GameModel gm, TowerInfo info, Vector3 position, bool upSide)
    {
        m_TowerInfo = info;
        m_Position = position;
        m_PriceText.text = m_TowerInfo.BasePrice.ToString();
        //判断金币是否足够
        m_GoldIsEnough = gm.Gold >= info.BasePrice;
        
        //加载对应的塔的图片
        string path = "TowerRoles/" + (m_GoldIsEnough ? info.EnableIcon : info.DisableIcon);

        m_SpriteRenderer.sprite = Resources.Load<Sprite>(path);

        //图标的显示位置是在上方还是下方        
        Vector3 locPosition = transform.localPosition;
        locPosition.y = upSide ? Mathf.Abs(locPosition.y) : -Mathf.Abs(locPosition.y);
        transform.localPosition = locPosition;
    }


    /// <summary>
    /// 生成炮塔
    /// </summary>
    void OnTowerClick()
    {
        SpawnTower();
    }


    void SpawnTower()
    {
        if (m_GoldIsEnough == false)
            return;

        int towerID = m_TowerInfo.TowerID;
        Vector3 postion = m_Position;

        //包装信息
        object[] objs = { towerID, postion };

        //向上层发送消息，最终TowerPopup脚本接收该消息并处理
        SendMessageUpwards("OnSpawnTower", objs, SendMessageOptions.RequireReceiver);
    }
}

