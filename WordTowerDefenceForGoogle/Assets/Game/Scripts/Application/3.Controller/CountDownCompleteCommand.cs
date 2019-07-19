using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CountDownCompleteCommand:Controller
{
    public override void Execute(object data)
    {
        //游戏开始
        GameModel gm = GetModel<GameModel>();
        gm.IsPlaying = true;
        
    }
}

