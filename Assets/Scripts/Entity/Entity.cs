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
    private UIBase healthSlideUI;
    protected TableAgent tab;
    public virtual void Init()
    {
        tab = new TableAgent();
        tab.Add(ResourceManager.Instance.Load<TextAsset>("Text/Table/Character").text);
        weaponManager = GetComponent<WeaponManager>();
        if (weaponManager != null)
            weaponManager.ent = this;
        healthSlideUI = new HealthSildeUI();
        healthSlideUI = UIManager.Instance.Open("HealthSlide");
        healthSlideUI.GetComponent<HealthSildeUI>().ent = this;
    }


    public virtual void Update()    {
        if (CurHealth <= 0)
        {
            UIManager.Instance.Close(healthSlideUI);
            Debug.Log("准备摧毁");
            Destroy(healthSlideUI.gameObject);
            Destroy(this.gameObject);

        }
    }

    
}
