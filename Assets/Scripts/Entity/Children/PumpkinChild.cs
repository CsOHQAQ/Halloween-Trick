using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinChild : ChildBase
{
    public override void Init()
    {
        base.Init();
        data.CurHealth = data.MaxHealth = tab.GetFloat("Character", "PumpkinChild", "Health");
        data.MoveSpeed = tab.GetFloat("Character", "PumpkinChild", "MoveSpeed");
        data.DPS = tab.GetFloat("Character", "PumpkinChild", "DPS");
        weaponManager.Add("ChPumpkin");
    }
}
