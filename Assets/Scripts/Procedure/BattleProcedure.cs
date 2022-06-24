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
    }
}
