using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1;
    public Vector2 direction = Vector2.zero;
    public string MasterName = "Player";
    public float Damage;
    public float LifeTime = 2;

    private float TimeCounter = 0;
    private void FixedUpdate()
    {
        if (TimeCounter <= LifeTime) TimeCounter += Time.deltaTime;
        else Destroy(this.gameObject);
    }
    void Update()
    {
        this.transform.Translate(direction.normalized * speed * Time.deltaTime);
    }
}
