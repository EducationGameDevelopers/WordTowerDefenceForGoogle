using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDesert : View {

    #region 字段
    private UISelect uiSelect;
    #endregion

    #region 属性
    public override string Name
    {
        get
        {
            return Consts.V_UIDesert;
        }
    }
    #endregion


    #region 事件回调
    public override void HandleEvent(string eventName, object data)
    {

    }
    #endregion

    #region Unity回调
    private void Awake()
    {
        uiSelect = this.transform.parent.GetComponent<UISelect>();
    }

    /// <summary>
    /// 返回按钮点击事件
    /// </summary>
    public void OnBackClick()
    {
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// 第一关点击事件,关卡索引从0开始
    /// </summary>
    public void OnLevel1Click()
    {
        uiSelect.ChooseLevel(60);
    }
    public void OnLevel2Click()
    {
        uiSelect.ChooseLevel(61);
    }
    public void OnLevel3Click()
    {
        uiSelect.ChooseLevel(62);
    }
    public void OnLevel4Click()
    {
        uiSelect.ChooseLevel(63);
    }
    public void OnLevel5Click()
    {
        uiSelect.ChooseLevel(64);
    }
    public void OnLevel6Click()
    {
        uiSelect.ChooseLevel(65);
    }
    public void OnLevel7Click()
    {
        uiSelect.ChooseLevel(66);
    }
    public void OnLevel8Click()
    {
        uiSelect.ChooseLevel(67);
    }
    public void OnLevel9Click()
    {
        uiSelect.ChooseLevel(68);
    }
    public void OnLevel10Click()
    {
        uiSelect.ChooseLevel(69);
    }
    public void OnLevel11Click()
    {
        uiSelect.ChooseLevel(70);
    }
    public void OnLevel12Click()
    {
        uiSelect.ChooseLevel(71);
    }
    public void OnLevel13Click()
    {
        uiSelect.ChooseLevel(72);
    }
    public void OnLevel14Click()
    {
        uiSelect.ChooseLevel(73);
    }
    public void OnLevel15Click()
    {
        uiSelect.ChooseLevel(74);
    }
    #endregion

}
