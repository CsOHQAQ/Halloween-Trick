using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGun : WeaponBase
{
    public override void Init()
    {
        base.Init();
        data = new WeaponData("HandGun");

    }
}
