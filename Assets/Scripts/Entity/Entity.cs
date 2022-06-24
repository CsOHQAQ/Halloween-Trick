using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

public class Entity :MonoBehaviour
{
    public float curHealth;
    public float MaxHealth;
    public float MoveSpeed;

    private UIBase HealthSlideUI;

    public virtual void Init()
    {

    }


    public virtual void Update()
    {
        if (curHealth <= 0)
        {
            DestroyImmediate(this.gameObject);

        }
    }

    
}
