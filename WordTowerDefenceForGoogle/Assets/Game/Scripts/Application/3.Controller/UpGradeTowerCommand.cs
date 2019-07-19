using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UpGradeTowerCommand:Controller
{
    public override void Execute(object data)
    {
        UpGradeTowerArgs e = data as UpGradeTowerArgs;
        Tower tower = e.Tower;

        GameModel gm = GetModel<GameModel>();
        
        if (gm.Gold >= tower.Price)
        {
            //金币数减少
            gm.Gold -= tower.Price;
            tower.Level++;
        }
        
    }
}

