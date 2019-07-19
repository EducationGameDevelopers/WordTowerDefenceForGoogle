using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIValcano : View {

    #region 字段
    private UISelect uiSelect;

    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_UIValcano; }
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
        uiSelect.ChooseLevel(0);
    }
    public void OnLevel2Click()
    {
        uiSelect.ChooseLevel(1);
    }
    public void OnLevel3Click()
    {
        uiSelect.ChooseLevel(2);
    }
    public void OnLevel4Click()
    {
        uiSelect.ChooseLevel(3);
    }
    public void OnLevel5Click()
    {
        uiSelect.ChooseLevel(4);
    }
    public void OnLevel6Click()
    {
        uiSelect.ChooseLevel(5);
    }
    public void OnLevel7Click()
    {
        uiSelect.ChooseLevel(6);
    }
    public void OnLevel8Click()
    {
        uiSelect.ChooseLevel(7);
    }
    public void OnLevel9Click()
    {
        uiSelect.ChooseLevel(8);
    }
    public void OnLevel10Click()
    {
        uiSelect.ChooseLevel(9);
    }
    public void OnLevel11Click()
    {
        uiSelect.ChooseLevel(10);
    }
    public void OnLevel12Click()
    {
        uiSelect.ChooseLevel(11);
    }
    public void OnLevel13Click()
    {
        uiSelect.ChooseLevel(12);
    }
    public void OnLevel14Click()
    {
        uiSelect.ChooseLevel(13);
    }
    public void OnLevel15Click()
    {
        uiSelect.ChooseLevel(14);
    }
    #endregion
}
