using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

public class Entity :MonoBehaviour
{
    public float CurHealth;
    public float MaxHealth;
    public float MoveSpeed;
    public float DPS;
    public WeaponManager weaponManager;

    private UIBase HealthSlideUI;
    protected TableAgent tab;
    public virtual void Init()
    {
        tab = new TableAgent();
        tab.Add(ResourceManager.Instance.Load<TextAsset>("Text/Table/Character").text);
        weaponManager = GetComponent<WeaponManager>();

    }


    public virtual void Update()    {
        if (CurHealth <= 0)
        {
            DestroyImmediate(this.gameObject);

        }
    }

    
}
