using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SellTowerCommand:Controller
{

    public override void Execute(object data)
    {
        SellTowerArgs e = data as SellTowerArgs;
        Tower tower = e.Tower;

        UIBoard uiBoard = GetView<UIBoard>();
        GameModel gm = GetModel<GameModel>();

        //游戏金币增加
        gm.Gold += (tower.Price / 2);
        uiBoard.Score = gm.Gold;
        //回收该塔
        Game.Instance.a_ObjectPool.Unspawn(tower.gameObject);
        //所在格子的数量清空
        tower.Tile.Data = null;
    }
}

