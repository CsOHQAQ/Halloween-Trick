using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBuffBase : Buff
{
    public EntityDataChanger buffChanger;
    
    public override void InitDataChange()
    {
        buffManager.entityChanger.maxHealthPcnt += buffChanger.maxHealthPcnt;
        buffManager.entityChanger.moveSpeedPcnt += buffChanger.moveSpeedPcnt;
    }
    public override void CancelDataChange()
    {
        base.CancelDataChange();
        buffManager.entityChanger.maxHealthPcnt -= buffChanger.maxHealthPcnt;
        buffManager.entityChanger.moveSpeedPcnt -= buffChanger.moveSpeedPcnt;
    }
}
