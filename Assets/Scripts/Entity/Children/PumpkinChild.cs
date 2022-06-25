using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinChild : ChildBase
{
    public override void Init()
    {
        base.Init();
        type = EntityType.PumpkinChild;
        CurHealth = MaxHealth = tab.GetFloat("Character", "PumpkinChild", "Health");
        MoveSpeed = tab.GetFloat("Character", "PumpkinChild", "MoveSpeed");
        DPS = tab.GetFloat("Character", "PumpkinChild", "DPS");
        weaponManager.Add("ChPumpkin");
    }
}
