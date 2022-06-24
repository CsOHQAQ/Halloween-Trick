using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

public class Entity :MonoBehaviour
{
    public float curHealth;
    public float MaxHealth;
    public float MoveSpeed;
    public WeaponManager weaponManager;

    private UIBase HealthSlideUI;
    protected TableAgent tab;
    public virtual void Init()
    {
        tab = new TableAgent();
        tab.Add(ResourceManager.Instance.Load<TextAsset>("Text/Table/Character").text);
        weaponManager = GetComponent<WeaponManager>();

    }



    private void Update()
    {
        if (curHealth <= 0)
        {
            DestroyImmediate(this.gameObject);

        }
    }

    
}
