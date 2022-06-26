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
		MapManager.Instance.Init();
		CardManager.Instance.Init();
        ResourceManager.Instance.Instantiate("Prefabs/Aimer");
        UIManager.Instance.Open("EnergySlide");
        UIManager.Instance.Open("HandCardSlot");
        UIManager.Instance.Open("TimerUI");
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
        UIManager.Instance.Close("TimerUI");
        EntityManager.Instance.ClearAll();
    }
}
