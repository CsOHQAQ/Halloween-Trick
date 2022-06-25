using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_ShotSpeed : WeaponBuffBase
{
    public Buff_ShotSpeed(float pcntIndex)
    {
        buffChanger = new WeaponDataChanger();
        buffChanger.shotSpeedPcnt = pcntIndex;
    }
}
