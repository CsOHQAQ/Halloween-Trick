using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBase : Entity
{
    private readonly float attackDistance = 1f; // 帧伤攻击距离
    private Animator animator;

    public override void Init()
    {
        base.Init();
        type = EntityType.BaseChild;
        CurHealth = MaxHealth = tab.GetFloat("Character", "BaseChild", "Health");
        MoveSpeed = tab.GetFloat("Character", "BaseChild", "MoveSpeed");
        DPS = tab.GetFloat("Character", "BaseChild", "DPS");
        animator = this.transform.GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();
        Vector2 playerPos = EntityManager.Instance.player.transform.position;

        // 向玩家靠近
        if(CurHealth>0)
        transform.position = Vector3.MoveTowards(transform.position, playerPos, MoveSpeed * Time.deltaTime);

        // 如果和玩家距离小于等于帧伤攻击距离 对玩家造成帧伤
        if (Vector2.Distance(transform.position, playerPos) <= attackDistance)
        {
            if (animator != null) animator.SetBool("IsAttack", true);
            EntityManager.Instance.player.CurHealth -= DPS * Time.deltaTime;
        }
        else
        {
            if (animator != null) animator.SetBool("IsAttack", false);
        }

        // 自动开火 往玩家方向
        if (weaponManager != null)
        {
            if (animator != null) animator.SetBool("IsAttack", true);
            for (int i = 0; i < weaponManager.EquipWeapon.Count; i++)
            {
                weaponManager.EquipWeapon[i].Fire(playerPos);
            }
        }


    }

    
}
