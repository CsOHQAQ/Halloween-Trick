using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

public class Entity :MonoBehaviour
{
    //public float CurHealth;
    //public float MaxHealth;
    //public float MoveSpeed;
    //public float DPS;

    private float lastMaxHealth;
    public EntityData data = new EntityData();
    public EntityDataBuffed Data = new EntityDataBuffed();

    public WeaponManager weaponManager;
    public BuffManager buffManager;

    private UIBase healthSlideUI;
    protected TableAgent tab;
    public EntityType type;
    private bool isDestorying = false;
    public virtual void Init()
    {
        tab = new TableAgent();
        tab.Add(ResourceManager.Instance.Load<TextAsset>("Text/Table/Character").text);
        weaponManager = GetComponent<WeaponManager>();
        if (weaponManager != null)
            weaponManager.ent = this;
        type = new EntityType();

        buffManager = GetComponent<BuffManager>();
        buffManager.ent = this;
        buffManager.Init();
        healthSlideUI = UIManager.Instance.Open("HealthSlide");
        healthSlideUI.GetComponent<HealthSildeUI>().ent = this;
        Data.data = data;
        Data.changer = buffManager.entityChanger;
    }

    public virtual void Update()    
    {
        if (Data.CurHealth <= 0)
        {
            if (gameObject.layer == 9 || gameObject.layer == 10)
            {
                if (!isDestorying)
                {
                    BeforeDestroy();
                    isDestorying = true;
                }
                
            }
        }
        
        if (Data.MaxHealth != lastMaxHealth && lastMaxHealth != 0)
        {
            Data.CurHealth += (Data.MaxHealth - lastMaxHealth);
            if (Data.CurHealth <= 0)
            {
                Data.CurHealth = 1;
            }
        }
        lastMaxHealth = Data.MaxHealth;
    }
    /// <summary>
    /// 执行摧毁前的操作 
    /// </summary>
    public virtual void BeforeDestroy()
    {
        DoDestory();
    }

    /// <summary>
    /// 真正执行摧毁的操作
    /// </summary>
    public void DoDestory()
    {

        UIManager.Instance.Close(healthSlideUI);
        try
        {
            Destroy(healthSlideUI.gameObject);
        }
        catch
        {
            Debug.LogWarning("未能成功销毁healthSlideUI");
        }
        try
        {
            Destroy(this.gameObject);
        }
        catch
        {
            Debug.LogWarning("未能成功销毁GO");
        }
        return;
    }
}

public class EntityData
{
    public float CurHealth;
    public float buffer1;
    public float MaxHealth;
    public float buffer2;
    public float buffer3;
    public float MoveSpeed;
    public float DPS;
}

public class EntityDataBuffed
{
    public EntityData data;
    public EntityDataChanger changer;

    public float CurHealth
    {
        get
        {
            return data.CurHealth;
        }
        set
        {
            data.CurHealth = value;
        }
    }
    public float MaxHealth => data.MaxHealth * (1 + changer.maxHealthPcnt);
    public float MoveSpeed => data.MoveSpeed * (1 + changer.moveSpeedPcnt);
    public float DPS => data.DPS * (1 + changer.DPSPcnt);
}

public enum EntityType
{
    Player,
    BaseChild,
    EggChild,
    PumpkinChild,
    TomatoChild,
    ChildKing,
    empty
}