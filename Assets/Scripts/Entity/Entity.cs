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
        healthSlideUI = new HealthSildeUI();
        healthSlideUI = UIManager.Instance.Open("HealthSlide");
        healthSlideUI.GetComponent<HealthSildeUI>().ent = this;
    }


    public virtual void Update()    {

        if (CurHealth <= 0)
        {
            UIManager.Instance.Close(healthSlideUI);
            Debug.Log("准备摧毁");
            if(healthSlideUI != null) Destroy(healthSlideUI.gameObject,2f);
            if(healthSlideUI!=null) healthSlideUI.gameObject.SetActive(false);
            if (this.GetComponent<Animator>() != null) this.GetComponent<Animator>().SetBool("IsDead", true);
            Destroy(this.gameObject,2f);
            if(this.transform.GetComponent<Rigidbody2D>()!=null) this.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            this.transform.GetComponent<BoxCollider2D>().isTrigger = true;
            if (this.transform.GetComponent<CircleCollider2D>()) this.transform.GetComponent<CircleCollider2D>().isTrigger = true;
        }
    }
}
