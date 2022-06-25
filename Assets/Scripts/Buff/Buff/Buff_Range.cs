using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Range : WeaponBuffBase
{
    public Buff_Range(float pcntIndex)
    {
        buffChanger = new WeaponDataChanger();
        buffChanger.rangePcnt = pcntIndex;
    }
}
