using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_FireTimes : WeaponBuffBase
{
    public Buff_FireTimes(int AddNum)
    {
        buffChanger = new WeaponDataChanger();
        buffChanger.fireTimesPlu = AddNum;
    }
}
