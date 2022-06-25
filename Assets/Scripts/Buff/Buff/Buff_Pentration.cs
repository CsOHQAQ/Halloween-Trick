using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Pentration : WeaponBuffBase
{
    public Buff_Pentration(int AddNum)
    {
        buffChanger = new WeaponDataChanger();
        buffChanger.pentrationPlu = AddNum;
    }
}
