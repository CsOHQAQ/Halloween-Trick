using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : WeaponBase
{
    public override void Init()
    {
        data = new WeaponData("MachineGun");

        base.Init();
    }
}
