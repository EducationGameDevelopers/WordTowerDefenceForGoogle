using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ExitSceneCommand : Controller
{
    public override void Execute(object data)
    {
        //对象池回收所有对象
        Game.Instance.a_ObjectPool.UnspawnAll();

        SceneArgs e = data as SceneArgs;
        if (e.SceneIndex == 3)
        {
            Time.timeScale = 1;
            RoundModel rm = GetModel<RoundModel>();
            GameModel gm = GetModel<GameModel>();
            //保存单词信息数据
            gm.SaveWordListToJson();

            rm.StopRound();
            rm.InitRound();
        }              
    }
}

