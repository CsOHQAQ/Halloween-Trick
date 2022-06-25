using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_BaseSpread : WeaponBuffBase
{
    public Buff_BaseSpread(float mulIndex)
    {
        buffChanger = new WeaponDataChanger();
        buffChanger.shotSpeedPcnt *= mulIndex;
    }
}
