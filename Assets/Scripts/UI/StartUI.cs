using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
using UnityEngine.UI;
public class StartUI : UIBase
{
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        GetComponent<Button>().onClick.AddListener(() =>
        {
            CardManager.Instance.Initialize();
            ProcedureManager.Instance.ChangeTo<BattleProcedure>();
        }
        );
    }
    protected override void OnClose()
    {
        base.OnClose();
    }
}