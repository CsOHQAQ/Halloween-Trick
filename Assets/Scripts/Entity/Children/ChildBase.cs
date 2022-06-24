using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBase : Entity
{
    private Vector2 pos = new Vector2();

    private void Start()
    {
        curHealth = 100;
        MoveSpeed = 1;
    }

    public override void Update()
    {
        base.Update();

        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = Vector3.MoveTowards(transform.position, pos, MoveSpeed * Time.deltaTime);
    }
}
