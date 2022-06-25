using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggChild : ChildBase
{
    public override void Init()
    {
        base.Init();
        CurHealth = MaxHealth = tab.GetFloat("Character", "EggChild", "Health");
        MoveSpeed = tab.GetFloat("Character", "EggChild", "MoveSpeed");
        DPS = tab.GetFloat("Character", "EggChild", "DPS");
        weaponManager.Add("ChEgg");
    }
}
