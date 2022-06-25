using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBuffBase : Buff
{
    public WeaponDataChanger buffChanger;
    
    public override void InitDataChange()
    {
        buffManager.weaponChanger.pentrationPlu += buffChanger.pentrationPlu;
        buffManager.weaponChanger.stopPowerPcnt += buffChanger.stopPowerPcnt;
        buffManager.weaponChanger.shotSpeedPcnt += buffChanger.shotSpeedPcnt;
        buffManager.weaponChanger.baseSpreadMul *= buffChanger.baseSpreadMul;
        buffManager.weaponChanger.fireTimesPlu += buffChanger.fireTimesPlu;
        buffManager.weaponChanger.fireCDMul *= buffChanger.fireCDMul;
        buffManager.weaponChanger.rangePlu += buffChanger.rangePlu;
    }
    public override void CancelDataChange()
    {
        base.CancelDataChange();
        buffManager.weaponChanger.pentrationPlu -= buffChanger.pentrationPlu;
        buffManager.weaponChanger.stopPowerPcnt -= buffChanger.stopPowerPcnt;
        buffManager.weaponChanger.shotSpeedPcnt -= buffChanger.shotSpeedPcnt;
        buffManager.weaponChanger.baseSpreadMul /= buffChanger.baseSpreadMul;
        buffManager.weaponChanger.fireTimesPlu -= buffChanger.fireTimesPlu;
        buffManager.weaponChanger.fireCDMul /= buffChanger.fireCDMul;
        buffManager.weaponChanger.rangePlu -= buffChanger.rangePlu;
    }
}
