using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 结束关卡命令
/// </summary>
public class EndLevelCommand : Controller
{
    public override void Execute(object data)
    {
        //命令参数转化成结束关卡类参数
        EndLevelArgs e = data as EndLevelArgs;

        //向游戏数据模型传递参数，结束关卡
        GameModel gm = GetModel<GameModel>();
        RoundModel rm = GetModel<RoundModel>();
        UserAnswerModel uam = GetModel<UserAnswerModel>();
    
        gm.StopLevel(e.IsWin);
        //对象池回收所有对象
        Game.Instance.a_ObjectPool.UnspawnAll();
        //显示对应UI界面
        if (e.IsWin == true)
        {
            GetView<UIWin>().Show();
           
        }
        else
        {
            GetView<UILost>().Show();
        }
        rm.StopRound();
        rm.InitRound();
    }
}

