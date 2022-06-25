﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

public class BattleProcedure : ProcedureBase
{
    protected override void OnEnter(object args)
    {
        base.OnEnter(args);
        EntityManager.Instance.Init();
        ResourceManager.Instance.Instantiate("Prefabs/Aimer");
    }

    protected override void OnLeave()
    {
        base.OnLeave();
        GameObject.DestroyImmediate(GameObject.Find("Aimer"));
    }
}