using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleProcedure : ProcedureBase {

    protected override void OnEnter(object args)
    {
        AddSubmodule(new Titlemodule());
        base.OnEnter(args);
<<<<<<< HEAD
=======

>>>>>>> 7d86ade8fb828566b54936221c200340cc0891ee
    }
}
