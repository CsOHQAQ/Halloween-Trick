using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_BaseSpread : WeaponBuffBase
{
    public Buff_BaseSpread(int mulIndex)
    {
        buffChanger = new WeaponDataChanger();
        buffChanger.shotSpeedPcnt *= mulIndex;
    }
}
