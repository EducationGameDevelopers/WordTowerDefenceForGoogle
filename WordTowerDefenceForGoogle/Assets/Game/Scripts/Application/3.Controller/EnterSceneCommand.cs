using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class EnterSceneCommand : Controller
{
    public override void Execute(object data)
    {
        //注册视图
        SceneArgs e = data as SceneArgs;
        Game.Instance.HideSceneLoadProcess();
        switch (e.SceneIndex)
        {
            case 0://Init

                break;

            case 1://Start
                RegisterView(GameObject.Find("UIStart").GetComponent<UIStart>());
                RegisterView(GameObject.Find("GameCanvas/UIBloodParent").GetComponent<UIBloodParent>());
                RegisterView(GameObject.Find("UIStart/UISelectLexicon").GetComponent<UISelectLexcion>());
                break;

            case 2://Select
                RegisterView(GameObject.Find("UISelect").GetComponent<UISelect>());
                break;

            case 3://Play
                Transform canvas = GameObject.Find("Canvas").transform;
                RegisterView(GameObject.Find("Map").GetComponent<Spawner>());
                RegisterView(GameObject.Find("TowerPopup").GetComponent<TowerPopup>());
                RegisterView(canvas.Find("UIBoard").GetComponent<UIBoard>());
                RegisterView(canvas.Find("UIWin").GetComponent<UIWin>());
                RegisterView(canvas.Find("UILost").GetComponent<UILost>());
                RegisterView(canvas.Find("UISystem").GetComponent<UISystem>());
                RegisterView(canvas.Find("UIAnswer").GetComponent<UIAnswer>());
                break;

            case 4://Complete
                RegisterView(GameObject.Find("UIComplete").GetComponent<UIComplete>());
                break;

            default:
                break;
        }
    }
}

