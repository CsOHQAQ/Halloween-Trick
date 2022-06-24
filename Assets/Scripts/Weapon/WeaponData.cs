using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Utilities;
public class WeaponData
{
    [Tooltip("名称")]
    public string Name;
    [Tooltip("穿透力，对障碍物的穿透能力")]
    public float Pentration;
    [Tooltip("停滞力，对人物的杀伤力")]
    public float StoppingPower;
    [Tooltip("质量，影响携带者移速")]
    public float Mass;
    [Tooltip("最大弹匣容量")]
    public int MaxAmmo;
    [Tooltip("当前弹药量")]
    public int CurAmmo;
    [Tooltip("基础装填时间，请搭配携带者装填速度共同食用")]
    public float ReloadTime;
    [Tooltip("后坐力，影响准星扩散以及移动")]
    public float BackForce;
    [Tooltip("射速，两次开火间的间隔")]
    public float ShotSpeed;
    [Tooltip("基础扩散，影响子弹发散程度")]
    public float BaseSpread;
    [Tooltip("开火模式")]
    protected FireMethod method;
    //下面是魔改加的东西
    [Tooltip("连续射击的次数")]
    public int FireTimes;
    [Tooltip("射击CD")]
    public float FireCD;

    public FireMethod Method
    {
        get
        {
            return method;
        }
    }
    private TableAgent table;
    public WeaponData(string Name)
    {
        table = new TableAgent();
        table.Add(Resources.Load<TextAsset>("Text/Table/"+Name).text);
        Pentration = table.GetFloat(Name, "Pentration","Data");
        StoppingPower=table.GetFloat(Name, "StoppingPower","Data");
        ShotSpeed= table.GetFloat(Name, "ShotSpeed", "Data"); 
        BaseSpread= table.GetFloat(Name, "BaseSpread", "Data");
        FireTimes = table.GetInt(Name, "FireTimes", "Data");
        FireCD = table.GetFloat(Name, "FireCD", "Data");
    }
}

public enum FireMethod
{
    Single,
    Auto,
}
public enum GunType
{
    HG,
    SMG,
    AR,
    RF,
    SG,
}