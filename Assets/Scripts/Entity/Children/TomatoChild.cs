using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoChild : ChildBase
{
    public override void Init()
    {
        base.Init();
        type = EntityType.TomatoChild;
        data.CurHealth = data.MaxHealth = tab.GetFloat("Character", "TomatoChild", "Health");
        data.MoveSpeed = tab.GetFloat("Character", "TomatoChild", "MoveSpeed");
        data.DPS = tab.GetFloat("Character", "TomatoChild", "DPS");
        attackDistance = tab.GetFloat("Character", "TomatoChild", "AttackDistance");
        weaponManager.Add("ChTomato");
        weaponManager.CurWeapon.bulletType = WeaponBase.BulletType.Tomato;
    }
}
