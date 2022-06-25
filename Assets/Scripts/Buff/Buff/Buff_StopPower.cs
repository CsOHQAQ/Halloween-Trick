using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_StopPower : WeaponBuffBase
{
    public Buff_StopPower(float pcntIndex)
    {
        buffChanger = new WeaponDataChanger();
        buffChanger.stopPowerPcnt = pcntIndex;
    }
}
