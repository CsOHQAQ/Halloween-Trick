using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

public class BattleProcedure : ProcedureBase
{
    protected override void OnEnter(object args)
    {
        base.OnEnter(args);
        EntityManager.Instance.Init();
        CardManager.Instance.Init();
        ResourceManager.Instance.Instantiate("Prefabs/Aimer");
        UIManager.Instance.Open("EnergySlide");
        UIManager.Instance.Open("HandCardSlot");
        GameDateTime night = new GameDateTime
        {
            Days = GameMgr.Get<IGameTimeManager>().GetNow().Days,
            Hours = 23,           
        };
        GameMgr.Get<IGameTimeManager>().StepMinute((night - GameMgr.Get<IGameTimeManager>().GetNow()).Minutes);
        GameMgr.Get<IGameTimeManager>().DoStart();
    }

    protected override void OnLeave()
    {
        base.OnLeave();
        GameObject.DestroyImmediate(GameObject.Find("Aimer"));
        UIManager.Instance.Close("EnergySlide");
        UIManager.Instance.Close("HandCardSlot");
        EntityManager.Instance.ClearAll();
    }
}
