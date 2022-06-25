using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoChild : ChildBase
{
    public override void Init()
    {
        base.Init();
        CurHealth = MaxHealth = tab.GetFloat("Character", "TomatoChild", "Health");
        MoveSpeed = tab.GetFloat("Character", "TomatoChild", "MoveSpeed");
        DPS = tab.GetFloat("Character", "TomatoChild", "DPS");
        weaponManager.Add("ChTomato");
    }
}
