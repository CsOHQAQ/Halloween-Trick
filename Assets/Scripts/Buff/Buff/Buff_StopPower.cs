using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_StopPower : WeaponBuffBase
{
    /// <summary>
    /// 请注意，此处的mulIndex对攻击系数的影响是线性叠加的，而非乘算
    /// </summary>
    /// <param name="mulIndex"></param>
    public Buff_StopPower(float mulIndex)
    {
        buffChanger = new WeaponDataChanger();
        buffChanger.stopPowerMul += mulIndex;

    }
    public override void InitDataChange()
    {
        buffManager.weaponChanger.stopPowerMul += buffChanger.stopPowerMul;
    }
    public override void CancelDataChange()
    {
        buffManager.weaponChanger.stopPowerMul -= buffChanger.stopPowerMul;
    }
}
