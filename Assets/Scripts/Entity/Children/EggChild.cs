using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggChild : ChildBase
{
    public override void Init()
    {
        base.Init();
        data.CurHealth = data.MaxHealth = tab.GetFloat("Character", "EggChild", "Health");
        data.MoveSpeed = tab.GetFloat("Character", "EggChild", "MoveSpeed");
        data.DPS = tab.GetFloat("Character", "EggChild", "DPS");
        weaponManager.Add("ChEgg");
    }
}
