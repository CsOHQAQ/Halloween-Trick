using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

public class ShopProcedure : ProcedureBase
{
    protected override void OnEnter(object args)
    {
        base.OnEnter(args);
        GameMgr.Get<IGameTimeManager>().Pause();

        UIManager.Instance.Open("ShopUI");

    }
    protected override void OnLeave()
    {
        base.OnLeave();
        UIManager.Instance.Close("ShopUI");
    }
}
