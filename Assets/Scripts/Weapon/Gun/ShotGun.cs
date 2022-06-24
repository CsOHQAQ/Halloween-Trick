using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : WeaponBase
{
    public override void Init()
    {
        base.Init();
        data = new WeaponData("MachineGun");

    }
}
