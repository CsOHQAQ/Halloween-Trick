using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_FireCD : WeaponBuffBase
{
    public Buff_FireCD(float mulIndex)
    {
        buffChanger = new WeaponDataChanger();
        buffChanger.fireCDMul *= mulIndex;
    }
}
