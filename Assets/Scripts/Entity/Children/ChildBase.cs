using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBase : Entity
{
    private readonly float attackDistance = 1f; // 帧伤攻击距离

    public override void Init()
    {
        base.Init();
        CurHealth = MaxHealth = tab.GetFloat("Character", "BaseChild", "Health");
        MoveSpeed = tab.GetFloat("Character", "BaseChild", "MoveSpeed");
        DPS = tab.GetFloat("Character", "BaseChild", "DPS");
    }

    private void Awake()
    {
        Init();
    }

    public override void Update()
    {
        base.Update();

        Vector2 playerPos = EntityManager.Instance.player.transform.position;

        // 向玩家靠近
        transform.position = Vector3.MoveTowards(transform.position, playerPos, MoveSpeed * Time.deltaTime);

        // 如果和玩家距离小于等于帧伤攻击距离 对玩家造成帧伤
        if (Vector2.Distance(transform.position, playerPos) <= attackDistance)
        {
            EntityManager.Instance.player.CurHealth -= DPS * Time.deltaTime;
        }

        // 自动开火 往玩家方向
        if(weaponManager!=null)
        for (int i = 0; i < weaponManager.EquipWeapon.Count; i++)
        {
            weaponManager.EquipWeapon[i].Fire(playerPos);
        }


    }

    
}
