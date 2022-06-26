using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildKing : ChildBase
{
    readonly float dashCoolDown = 8f; //冲刺技能冷却
    private float timer = 0f; //冲刺计时器
    private Rigidbody2D body;
    public override void Init()
    {
        base.Init();
        type = EntityType.ChildKing;
        data.CurHealth = data.MaxHealth = tab.GetFloat("Character", "ChildKing", "Health");
        data.MoveSpeed = tab.GetFloat("Character", "ChildKing", "MoveSpeed");
        data.DPS = tab.GetFloat("Character", "ChildKing", "DPS");
        attackDistance = tab.GetFloat("Character", "ChildKing", "AttackDistance");
        body = GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;
        Vector2 playerPos = EntityManager.Instance.player.transform.position;
        if (timer >= dashCoolDown 
            && Vector2.Distance(transform.position, playerPos) > attackDistance) //冷却完毕且距离玩家较远 发动冲刺
        {
            if (animator != null)
            {
                animator.SetBool("IsWalk", false);
            }

            //body.AddForce((playerPos - (Vector2)transform.position) * 800); //离得越远冲的越远
            body.AddForce((playerPos - (Vector2)transform.position).normalized * 6000); //固定距离冲
            timer = 0f;
        }
    }

    public override void BeforeDestroy()
    {
        Debug.Log(name + "濒死");

        EntityManager.Instance.AddEnergy(this);
        animator.SetBool("IsDead", true);

    }

    // 创死其他小朋友
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9) // 创到其他小朋友 直接创死
        {
            collision.GetComponent<Entity>().BeforeDestroy();
            if (animator != null) animator.SetTrigger("IsAttack");
            animator.SetBool("IsWalk", true);
        }
        else if (collision.gameObject.layer == 8) // 创到玩家 造成DPS的伤害
        {
            if (body.velocity.magnitude > 0.2f) // 速度大于0.2认为在冲撞过程中
            {
                collision.GetComponent<Entity>().Data.CurHealth -= Data.DPS;
                if (animator != null) animator.SetTrigger("IsAttack");
                animator.SetBool("IsWalk", true);
            }
        }
    }
}
