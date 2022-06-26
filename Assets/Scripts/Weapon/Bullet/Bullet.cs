using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Bullet : MonoBehaviour
{
    public float speed = 1;
    public Vector2 direction = Vector2.zero;
    public Vector2 Start;
    public float Range = 0;
    public float ColorChangeSpeed=50;
    public GameObject NewEffect,
                                    LeftThings;
    public WeaponBase.BulletType BulletType;

    public string MasterName = "Player";
    public int layer;
    public float Damage;
    public float Pentration;


    private float TimeCounter = 0;
    private Color32 selfColor;
    private float TransValue = 125;

    private void Awake()
    {
        if (this.transform.GetChild(0).GetComponent<TrailRenderer>() != null)
        {
            switch (Random.Range(1, 6))
            {
                case 1:
                    selfColor= new Color32((byte)255, (byte)0, (byte)0, (byte)TransValue);
                    this.transform.GetChild(0).GetComponent<TrailRenderer>().startColor = selfColor;
                    this.transform.GetComponent<SpriteRenderer>().color = new Color32(selfColor.r, selfColor.g, selfColor.b, 255);

                    break;
                case 2:
                    selfColor=new Color32((byte)255, (byte)255, (byte)0, (byte)TransValue);
                    this.transform.GetChild(0).GetComponent<TrailRenderer>().startColor = selfColor;
                    this.transform.GetComponent<SpriteRenderer>().color = new Color32(selfColor.r, selfColor.g, selfColor.b, 255);
                    break;
                case 3:
                    selfColor=new Color32((byte)0, (byte)255, (byte)0, (byte)TransValue);
                    this.transform.GetChild(0).GetComponent<TrailRenderer>().startColor = selfColor;
                    this.transform.GetComponent<SpriteRenderer>().color = new Color32(selfColor.r, selfColor.g, selfColor.b, 255);
                    break;
                case 4:
                    selfColor=new Color32((byte)0, (byte)255, (byte)255, (byte)TransValue);
                    this.transform.GetChild(0).GetComponent<TrailRenderer>().startColor = selfColor;
                    this.transform.GetComponent<SpriteRenderer>().color = new Color32(selfColor.r, selfColor.g, selfColor.b, 255);
                    break;
                case 5:
                    selfColor=new Color32((byte)0, (byte)0, (byte)255, (byte)TransValue);
                    this.transform.GetChild(0).GetComponent<TrailRenderer>().startColor = selfColor;
                    this.transform.GetComponent<SpriteRenderer>().color = new Color32(selfColor.r, selfColor.g, selfColor.b, 255);
                    break;
                case 6:
                    selfColor=new Color32((byte)255, (byte)0, (byte)255, (byte)TransValue);
                    this.transform.GetChild(0).GetComponent<TrailRenderer>().startColor = selfColor;
                    this.transform.GetComponent<SpriteRenderer>().color = new Color32(selfColor.r, selfColor.g, selfColor.b, 255);
                    break;
            }
            this.transform.GetChild(0).GetComponent<TrailRenderer>().endColor = selfColor;
            this.transform.GetChild(1).GetComponent<Light2D>().color = new Color32(selfColor.r, selfColor.g, selfColor.b, 255);
        }
    }

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
                collision.GetComponent<Entity>().Data.CurHealth -= Damage;
                collision.attachedRigidbody.AddForce(direction.normalized * Pentration * 10);
                Pentration--;
                GameObject newEffect = GameObject.Instantiate(NewEffect);
                GameObject newLeft = GameObject.Instantiate(LeftThings);
                newLeft.transform.position=newEffect.transform.position = this.transform.position;
                newLeft.GetComponent<SpriteRenderer>().color = selfColor;
                newEffect.GetComponent<ParticleSystem>().startColor=selfColor;
                Destroy(newEffect, 2);
            }
        }
        else if (layer == 9 || layer == 10)//开枪人是敌人
        {
            if (collision.gameObject.layer == 8 && collision.isTrigger)
            {
                collision.GetComponent<Entity>().Data.CurHealth -= Damage;
                collision.attachedRigidbody.AddForce(direction.normalized * Pentration * 10);
                Pentration--;
                GameObject newEffect = GameObject.Instantiate(NewEffect);
                GameObject newLeft = GameObject.Instantiate(LeftThings);
                newLeft.transform.position = newEffect.transform.position = this.transform.position;
                newLeft.GetComponent<SpriteRenderer>().color = selfColor;
                newEffect.GetComponent<ParticleSystem>().startColor = selfColor;
                Destroy(newEffect, 2);
            }

        }
        if (Pentration <= 0)
        {
            onDestory();
        }
    }
}
