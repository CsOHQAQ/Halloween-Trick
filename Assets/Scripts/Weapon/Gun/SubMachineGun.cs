using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMachineGun : WeaponBase
{
    public override void Init()
    {
        data = new WeaponData("SubMachineGun");

        base.Init();
    }
}
