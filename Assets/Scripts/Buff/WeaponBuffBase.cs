using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBuffBase : Buff
{
    public WeaponDataChanger buffChanger;
    
    public override void InitDataChange()
    {
        buffManager.weaponChanger.pentrationPlu += buffChanger.pentrationPlu;
        buffManager.weaponChanger.stopPowerMul*= buffChanger.stopPowerMul;
        buffManager.weaponChanger.shotSpeedMul *= buffChanger.shotSpeedMul;
        buffManager.weaponChanger.baseSpreadMul *= buffChanger.baseSpreadMul;
        buffManager.weaponChanger.fireTimesPlu += buffChanger.fireTimesPlu;
        buffManager.weaponChanger.fireCDMul *= buffChanger.fireCDMul;
        buffManager.weaponChanger.rangeMul *= buffChanger.rangeMul;
    }
    public override void CancelDataChange()
    {
        base.CancelDataChange();
        buffManager.weaponChanger.pentrationPlu -= buffChanger.pentrationPlu;
        buffManager.weaponChanger.stopPowerMul /= buffChanger.stopPowerMul;
        buffManager.weaponChanger.shotSpeedMul /= buffChanger.shotSpeedMul;
        buffManager.weaponChanger.baseSpreadMul /= buffChanger.baseSpreadMul;
        buffManager.weaponChanger.fireTimesPlu -= buffChanger.fireTimesPlu;
        buffManager.weaponChanger.fireCDMul /= buffChanger.fireCDMul;
        buffManager.weaponChanger.rangeMul /= buffChanger.rangeMul;
    }
}
