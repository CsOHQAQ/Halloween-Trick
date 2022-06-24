using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;
public class BuffManager : MonoBehaviour
{
    public Timeline time;
    public  List<Buff> BuffList;

    private List<Buff> DisposeList;
    public HumanBase human;

    public void Init()
    {
        time = GetComponent<Timeline>();
        human = GetComponent<HumanBase>();
        time.globalClockKey = "InGame";
        BuffList = new List<Buff>();
        DisposeList = new List<Buff>();
    }
    protected virtual void FixedUpdate()
    {
        
    }
    protected virtual void Update()
    {
        foreach(var buff in BuffList)
        {
            buff.Refresh();
            if (buff.LastingTime <= 0)
            {
                DisposeList.Add(buff);
            }
        }
        foreach(var t in DisposeList)
        {
            t.BeforeRemove();
            BuffList.Remove(t);

        }
           
        DisposeList.Clear();
    }

    //这个需要填写添加buff的种类与所加buff的积累值
    public void AddBuff(Buff buff,float AddTime,float AddCount=0)
    {
        foreach(var b in BuffList)
        {
            if (b.GetType() ==buff.GetType() )
            {
                b.LastingTime += AddTime;
                b.Count += AddCount;
                if (b.Count > b.MaxCount)
                    b.ActivateWhenFull();
                return;
            }
        }

        buff.SetManager(this);
        buff.Count = AddCount;
        buff.LastingTime = AddTime;
        buff.Init();
        BuffList.Add(buff);
        
    }   

    //移除buff用,不要直接remove会无法消除影响
    public void RemoveBuff<BuffType>()
    {

        foreach(var buff in BuffList)
        {
            if (buff is BuffType)
            {
                DisposeList.Add(buff);
                return;
            }
        }
        
       
    }

    public void RemoveAllBuff()
    {
        foreach (var buff in BuffList)
        {
            buff.ClearEffect();
        }
        BuffList.Clear();
    } 
    public List <Buff> CurrentBuff()
    {
        return BuffList;
    }

}

public class DataChanger
{
    //变量名以mul结尾表示乘算，plu表示加算
    public float HealthPlu;
    public float AccMul;
    public float MoveSpeedMul;    
    public float EudRecoverSpeedMul;    
    public float HealSpeedMul;
    public float ReloadSpeedMul;
    public float AccuracyMul;
    public float AimSpeedMul;
    public DataChanger()
    {
        MoveSpeedMul = 1;
        HealthPlu = 0;
        EudRecoverSpeedMul = 1;
        AccMul = 1;
        HealSpeedMul = 1;
        ReloadSpeedMul = 1;
        AccuracyMul = 1;
        AimSpeedMul = 1;

    }
}