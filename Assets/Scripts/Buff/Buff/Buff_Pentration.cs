using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Pentration : Buff
{
    public int AddNum = 0;
    public override void InitDataChange()
    {
        buffManager.weaponChanger.pentrationPlu += AddNum;
    }
    public override void CancelDataChange()
    {
        buffManager.weaponChanger.pentrationPlu -= AddNum;
    }
}
