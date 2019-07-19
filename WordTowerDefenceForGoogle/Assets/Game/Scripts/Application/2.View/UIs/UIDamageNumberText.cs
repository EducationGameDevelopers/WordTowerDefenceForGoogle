using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIDamageNumberText : ResuableObject, IReusable
{

    #region 字段
    private Text damageNumberText;
    #endregion
    #region Unity回调
    private void Awake()
    {
        damageNumberText = GetComponent<Text>();
    }
    #endregion
    #region 回调
    public override void OnSpawn()
    {
        
    }

    public override void OnUnspawn()
    {
        
    }
    #endregion
    #region 方法
    public void ShowDamageNumber(int hpChange, GameObject go)
    {
        damageNumberText = this.gameObject.GetComponent<Text>();
        damageNumberText.text = "-" + hpChange.ToString();
        damageNumberText.transform.DOScale(1.5f, 0.2f);
        Tweener tween = damageNumberText.transform.DOLocalMove(go.gameObject.transform.localPosition + new Vector3(0, 20, 0), 0.2f);
        tween.OnComplete(() => ResetDamageNumber());
    }

    public void ResetDamageNumber()
    {      
        Game.Instance.a_ObjectPool.Unspawn(this.gameObject);
    }
    #endregion
}
