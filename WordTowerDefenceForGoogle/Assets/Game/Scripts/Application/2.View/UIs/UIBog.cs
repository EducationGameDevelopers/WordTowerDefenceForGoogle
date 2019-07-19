using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBog : View {

    #region 字段
    private UISelect uiSelect;

    public override string Name
    {
        get
        {
            return Consts.V_UIBog;
        }
    }

    #endregion

    #region 属性

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
        uiSelect.ChooseLevel(15);
    }
    public void OnLevel2Click()
    {
        uiSelect.ChooseLevel(16);
    }
    public void OnLevel3Click()
    {
        uiSelect.ChooseLevel(17);
    }
    public void OnLevel4Click()
    {
        uiSelect.ChooseLevel(18);
    }
    public void OnLevel5Click()
    {
        uiSelect.ChooseLevel(19);
    }
    public void OnLevel6Click()
    {
        uiSelect.ChooseLevel(20);
    }
    public void OnLevel7Click()
    {
        uiSelect.ChooseLevel(21);
    }
    public void OnLevel8Click()
    {
        uiSelect.ChooseLevel(22);
    }
    public void OnLevel9Click()
    {
        uiSelect.ChooseLevel(23);
    }
    public void OnLevel10Click()
    {
        uiSelect.ChooseLevel(24);
    }
    public void OnLevel11Click()
    {
        uiSelect.ChooseLevel(25);
    }
    public void OnLevel12Click()
    {
        uiSelect.ChooseLevel(26);
    }
    public void OnLevel13Click()
    {
        uiSelect.ChooseLevel(27);
    }
    public void OnLevel14Click()
    {
        uiSelect.ChooseLevel(28);
    }
    public void OnLevel15Click()
    {
        uiSelect.ChooseLevel(14);
    }
    #endregion
}
