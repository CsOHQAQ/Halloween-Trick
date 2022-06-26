using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class StartTitleProcedure : ProcedureBase
{
    protected override void OnInit()
    {
        base.OnInit();
    }
    protected override void OnEnter(object args)
    {
        base.OnEnter(args);

        App.Common.Data.Instance.SetTableAgent();
        GameMgr.Instance.InitModules();
        UIManager.Instance.Open("StartUI");
        GameMgr.Get<IGameTimeManager>().Pause();

    }
    protected override void OnLeave()
    {
        base.OnLeave();
        UIManager.Instance.Close("StartUI");
    }
}
