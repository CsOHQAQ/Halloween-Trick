using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoChild : ChildBase
{
    public override void Init()
    {
        base.Init();
        data.CurHealth = data.MaxHealth = tab.GetFloat("Character", "TomatoChild", "Health");
        data.MoveSpeed = tab.GetFloat("Character", "TomatoChild", "MoveSpeed");
        data.DPS = tab.GetFloat("Character", "TomatoChild", "DPS");
        weaponManager.Add("ChTomato");
    }
}
