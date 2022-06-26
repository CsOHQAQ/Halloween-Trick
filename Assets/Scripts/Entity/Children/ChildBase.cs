using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBase : Entity
{
    public float attackDistance;
    protected Animator animator;

    public override void Init()
    {
        base.Init();
        type = EntityType.BaseChild;
        data.CurHealth = data.MaxHealth = tab.GetFloat("Character", "BaseChild", "Health");
        data.MoveSpeed = tab.GetFloat("Character", "BaseChild", "MoveSpeed");
        data.DPS = tab.GetFloat("Character", "BaseChild", "DPS");
        attackDistance = tab.GetFloat("Character", "BaseChild", "AttackDistance");
        animator = this.transform.GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();
        Vector2 playerPos = EntityManager.Instance.player.transform.position;

        // 向玩家靠近
        if (Data.CurHealth > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPos, Data.MoveSpeed * Time.deltaTime);
        }

        // 如果和玩家距离小于等于帧伤攻击距离 对玩家造成帧伤
        if (Vector2.Distance(transform.position, playerPos) <= attackDistance)
        {
            if (animator != null) animator.SetBool("IsAttack", true);
            EntityManager.Instance.player.Data.CurHealth -= Data.DPS * Time.deltaTime;
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
    public override void BeforeDestroy()
    {
        Debug.Log(name + "濒死");

        EntityManager.Instance.AddEnergy(this);
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsDead", true);

    }

}
