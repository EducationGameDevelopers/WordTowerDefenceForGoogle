using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBloodParent:View {
   
    public override string Name
    {
        get { return Consts.V_UIBloodParent; }
    }

    public override void RegisterAttentionEvent()
    {
        AttentionEventList.Add(Consts.E_BloodFollow);   //血条跟随事件
        AttentionEventList.Add(Consts.E_GoldEffect);    //金币特效事件
    }

    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case Consts.E_BloodFollow:
                BloodFollowArgs e = data as BloodFollowArgs;
                //对象池加载血条
                Game.Instance.a_ObjectPool.ResourcesDir = "Prefabs";
                GameObject go = Game.Instance.a_ObjectPool.Spawn("BloodBar");
                go.transform.SetParent(this.transform);

                //血条跟随目标
                BloodBar bloodBarShow = go.GetComponent<BloodBar>();
                bloodBarShow.FollowTarget = e.FollowTarget;
                bloodBarShow.RelevantMonster = e.RelevantMonster;
                break;

                //处理金币特效事件，BloodBar,UIAnswer脚本发出
            case Consts.E_GoldEffect:
                ItemPositionArgs e1 = data as ItemPositionArgs;
                //对象池加载金币预制体
                Game.Instance.a_ObjectPool.ResourcesDir = "Prefabs/Items";
                GameObject goldGO = Game.Instance.a_ObjectPool.Spawn("Gold");
                goldGO.transform.SetParent(this.transform);
                //金币出现位置设置
                goldGO.transform.position = e1.ItemPostion;         
                break;

            default:
                break;
        }
    }
}
