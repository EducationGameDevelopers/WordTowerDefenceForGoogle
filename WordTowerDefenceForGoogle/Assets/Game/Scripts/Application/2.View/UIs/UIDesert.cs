using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISnowMountain : View {

    #region 字段
    private UISelect uiSelect;
    #endregion

    #region 属性
    public override string Name
    {
        get
        {
            return Consts.V_UISnowMountain;
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
        uiSelect.ChooseLevel(75);
    }
    public void OnLevel2Click()
    {
        uiSelect.ChooseLevel(76);
    }
    public void OnLevel3Click()
    {
        uiSelect.ChooseLevel(77);
    }
    public void OnLevel4Click()
    {
        uiSelect.ChooseLevel(78);
    }
    public void OnLevel5Click()
    {
        uiSelect.ChooseLevel(79);
    }
    public void OnLevel6Click()
    {
        uiSelect.ChooseLevel(80);
    }
    public void OnLevel7Click()
    {
        uiSelect.ChooseLevel(81);
    }
    public void OnLevel8Click()
    {
        uiSelect.ChooseLevel(82);
    }
    public void OnLevel9Click()
    {
        uiSelect.ChooseLevel(83);
    }
    public void OnLevel10Click()
    {
        uiSelect.ChooseLevel(84);
    }
    public void OnLevel11Click()
    {
        uiSelect.ChooseLevel(85);
    }
    public void OnLevel12Click()
    {
        uiSelect.ChooseLevel(86);
    }
    public void OnLevel13Click()
    {
        uiSelect.ChooseLevel(87);
    }
    public void OnLevel14Click()
    {
        uiSelect.ChooseLevel(88);
    }
    public void OnLevel15Click()
    {
        uiSelect.ChooseLevel(89);
    }
    #endregion

}
