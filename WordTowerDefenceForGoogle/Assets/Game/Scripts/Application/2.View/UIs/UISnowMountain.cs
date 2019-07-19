using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINight : View {

    #region 字段
    private UISelect uiSelect;
    #endregion

    #region 属性
    public override string Name
    {
        get
        {
            return Consts.V_UINight;
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
        uiSelect.ChooseLevel(45);
    }
    public void OnLevel2Click()
    {
        uiSelect.ChooseLevel(46);
    }
    public void OnLevel3Click()
    {
        uiSelect.ChooseLevel(47);
    }
    public void OnLevel4Click()
    {
        uiSelect.ChooseLevel(48);
    }
    public void OnLevel5Click()
    {
        uiSelect.ChooseLevel(49);
    }
    public void OnLevel6Click()
    {
        uiSelect.ChooseLevel(50);
    }
    public void OnLevel7Click()
    {
        uiSelect.ChooseLevel(51);
    }
    public void OnLevel8Click()
    {
        uiSelect.ChooseLevel(52);
    }
    public void OnLevel9Click()
    {
        uiSelect.ChooseLevel(53);
    }
    public void OnLevel10Click()
    {
        uiSelect.ChooseLevel(54);
    }
    public void OnLevel11Click()
    {
        uiSelect.ChooseLevel(55);
    }
    public void OnLevel12Click()
    {
        uiSelect.ChooseLevel(56);
    }
    public void OnLevel13Click()
    {
        uiSelect.ChooseLevel(57);
    }
    public void OnLevel14Click()
    {
        uiSelect.ChooseLevel(58);
    }
    public void OnLevel15Click()
    {
        uiSelect.ChooseLevel(59);
    }
    #endregion

}
