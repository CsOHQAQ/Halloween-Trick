using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1;
    public Vector2 direction = Vector2.zero;
    public Vector2 Start;
    public float Range = 0;

    public string MasterName = "Player";
    public int layer;
    public float Damage;
    public float Pentration;


    private float TimeCounter = 0;
    void Update()
    {
        this.transform.Translate(direction.normalized * speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, Start) >= Range)
        {
            onDestory();
        }
    }
    public void onDestory()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 判断碰撞体是trigger才造成伤害 防止重复判定
        if (layer == 8)//开枪人是玩家
        {
            if ((collision.gameObject.layer == 9 || collision.gameObject.layer == 10)
                && collision.isTrigger)
            {
                collision.GetComponent<Entity>().CurHealth -= Damage;
                collision.attachedRigidbody.AddForce(direction.normalized * Pentration * 10);
                Pentration--;

            }
        }
        else if (layer == 9 || layer == 10)//开枪人是敌人
        {
            if (collision.gameObject.layer == 8 && collision.isTrigger)
            {
                collision.GetComponent<Entity>().CurHealth -= Damage;
                collision.attachedRigidbody.AddForce(direction.normalized * Pentration * 10);
                Pentration--;

            }

        }
        if (Pentration <= 0)
        {
            onDestory();
        }
    }
}
