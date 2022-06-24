using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

/// <summary>
/// 目标是可以根据HumanData生成出对应的HumanBase
/// 同时需要保证可以被序列化，用于后续保存
/// </summary>

public class HumanData
{

    //以下都是需要读表进行初始化的数据
    [Tooltip("编程用名称")]
    public string CodeName;
    [Tooltip("名字，应当是游戏内称呼的中文名")]
    public string Name;
    [Tooltip("生命上限，字面含义")]
    private float maxHealth;
    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }
    
    [Tooltip("体力上限，执行某些行为时消耗。由生命上限影响")]
    public float MaxEndurance;
    [Tooltip("移速上限，影响移动与冲刺的速度")]
    private  float moveSpeed;
    public float MoveSpeed
    {
        get
        {
            //if (dataChanger.MoveSpeedMul > 1)
                //Debug.Log("正在跑步");
            return dataChanger.MoveSpeedMul * moveSpeed;
        }
    }
    [Tooltip("加速度，影响移动的提速")]
    private float acceleration;
    public float Acceleration
    {
        get
        {
            return dataChanger.AccMul * acceleration;
        }
    }
    [Tooltip("治疗速度，影响执行治疗行为的速度")]
    private float healSpeed;
    public float HealSpeed
    {
        get
        {
            return dataChanger.HealSpeedMul * healSpeed;
        }
    }
    [Tooltip("装填速度，影响装填子弹的速度")]
    private float reloadSpeed;
    public float ReloadSpeed
    {
        get
        {
            return dataChanger.ReloadSpeedMul * reloadSpeed;
        }
    }
    [Tooltip("瞄准速度，影响准星移动速度（比如转头）以及完成瞄准的速度（即准星收缩至最大程度的速度）")]
    private float aimSpeed;
    public float AimSpeed
    {
        get
        {
            return dataChanger.AimSpeedMul * aimSpeed;
        }
    }

    [Tooltip("瞄准精度，影响准星收缩程度")]
    public float Accuracy;
    [Tooltip("背包容量，决定能随身携带多少东西")]
    public int CargoSize;

    [Tooltip("最大饱食度，人物当前最大饱食度")]
    private float maxHunger;
    public float MaxHunger
    {
        get
        {
            return maxHunger;
        }
        set
        {
            maxHunger = value;
        }
    }

    [Tooltip("饱食度消耗速度")]
    public float ConsumeSpeed;
    [Tooltip("恢复速度")]
    public float RecoverSpeed;

    //以下更多适用于会存读档的数据

    

    [Tooltip("真实血量")]
    private float realHealth;
    [Tooltip("虚血")]
    private float virtualHealth;
    [Tooltip("虚血掉血速度")]
    public float VHealthDropSpd;

    [Tooltip("当前体力")]
    public float CurrentEndurance;
    public DataChanger dataChanger;

    [Tooltip("背包数据")]
    public CargoData backpack;
    [Tooltip("人物携带的武器数据")]
    public List<WeaponData> weaponList;
    [Tooltip("当前携带的Buff栏")]
    public List<Buff> buffList;

    public float CurrentHunger;

    public float CurrentHealth => realHealth + virtualHealth;




    private TableAgent tab;

    /// <summary>
    /// 使用人物名字初始化一套HumanData
    /// </summary>
    /// <param name="name">名字</param>
    public HumanData(string name = "NormalHuman")
    {
        tab = new TableAgent();
        tab.Add(ResourceManager.Instance.Load<TextAsset>("Text/Table/HumanData").text);
        if (!tab.CollectKey1("HumanData").Contains(name))
        {
            Debug.LogWarning("人物表中未找到" + name+",已设置为默认人物");
            name = "NormalHuman";
        }
        CodeName = name;
        Name = tab.GetString("HumanData", name, "Name");
        maxHealth = tab.GetFloat("HumanData",name, "MaxHealth");
        VHealthDropSpd = tab.GetFloat("HumanData", name, "VHealthDropSpd");
        MaxEndurance = tab.GetFloat("HumanData", name, "MaxEndurance"); 
        moveSpeed = tab.GetFloat("HumanData",  name, "MoveSpeed");
        acceleration = tab.GetFloat("HumanData", name, "Accleration");
        healSpeed = tab.GetFloat("HumanData", name, "HealSpeed");
        reloadSpeed = tab.GetFloat("HumanData", name, "ReloadSpeed");
        aimSpeed = tab.GetFloat("HumanData", name, "AimSpeed");
        Accuracy = tab.GetFloat("HumanData", name, "Accuracy");
        CargoSize = tab.GetInt("HumanData", name, "CargoSize");
        maxHunger = tab.GetFloat("HumanData", name, "MaxHunger");
        ConsumeSpeed = tab.GetFloat("HumanData", name, "ConsumeSpeed");
        RecoverSpeed = tab.GetFloat("HumanData", name, "RecoverSpeed");
        dataChanger =new DataChanger();

        backpack = new CargoData();
        backpack.CargoName = name;
        backpack.MaxBattery = CargoSize;
    }
    public void ResetState()
    {
        realHealth = maxHealth;
        CurrentEndurance = MaxEndurance;
        CurrentHunger = maxHunger;
    }
    public void MaxHpChange(float h)
    {
        maxHealth += h;
        if (realHealth > maxHealth)
            realHealth = maxHealth;
    }

    public void MaxEdChange(float e)
    {
        MaxEndurance += e;
        if (CurrentEndurance > MaxEndurance)
            CurrentEndurance = MaxEndurance;
    }

    public void HpChange(float h, HpChangeState mode)
    {
        //realHealth += h;
        //if (realHealth > maxHealth) realHealth = maxHealth;
        //if (realHealth < 0) realHealth = 0;

        if (mode == HpChangeState.RealHp)
        {
            realHealth += h;
            if (CurrentHealth > maxHealth) realHealth = maxHealth - virtualHealth;
            if (realHealth < 0) realHealth = 0;
        }
        else if (mode == HpChangeState.VirtualHp)
        {
            virtualHealth += h;
            if (CurrentHealth > maxHealth) virtualHealth = maxHealth - realHealth;
            if (virtualHealth < 0) virtualHealth = 0;
        }
        else
        {
            virtualHealth += h;
            if (CurrentHealth > maxHealth) virtualHealth = maxHealth - realHealth;
            if (virtualHealth < 0)
            {
                realHealth += virtualHealth; //如果virtualHealth变成负数了，那么其绝对值就是真实血量应该扣的值
                virtualHealth = 0;
                if (realHealth < 0) realHealth = 0;
            }
        }
    }
    
    public void EnduranceChange(float e)
    {
        CurrentEndurance += e;
        if (CurrentEndurance > MaxEndurance) CurrentEndurance = MaxEndurance;
        if (CurrentEndurance < 0) CurrentEndurance = 0;
    }


}

public enum HpChangeState
{
    RealHp,         //变化真实血量
    VirtualHp,      //变化虚血
    AllHp,          //先变化虚血，耗尽后变化实血
}
