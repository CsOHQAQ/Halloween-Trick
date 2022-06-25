using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_StopPower : WeaponBuffBase
{
    public Buff_StopPower(float mulIndex)
    {
        buffChanger.stopPowerMul += mulIndex;

    }
    public override void InitDataChange()
    {
        base.InitDataChange();
    }
}
