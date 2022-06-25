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
    public BuffManager buffManager;

    private UIBase healthSlideUI;
    protected TableAgent tab;
    public virtual void Init()
    {
        tab = new TableAgent();
        tab.Add(ResourceManager.Instance.Load<TextAsset>("Text/Table/Character").text);
        weaponManager = GetComponent<WeaponManager>();
        if (weaponManager != null)
            weaponManager.ent = this;
        buffManager = GetComponent<BuffManager>();
        buffManager.ent = this;
        buffManager.Init();
        healthSlideUI = UIManager.Instance.Open("HealthSlide");
        healthSlideUI.GetComponent<HealthSildeUI>().ent = this;
    }


    public virtual void Update()    {

        if (CurHealth <= 0)
        {
            UIManager.Instance.Close(healthSlideUI);
            try
            {
                Destroy(healthSlideUI.gameObject);
            }
            catch {
                Debug.LogWarning("未能成功销毁healthSlideUI");
            }
            try
            {
                Destroy(this.gameObject);
            }
            catch {
                Debug.LogWarning("未能成功销毁GO");
            }

        }
    }


}
