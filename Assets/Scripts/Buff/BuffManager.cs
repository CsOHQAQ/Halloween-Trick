using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BuffManager : MonoBehaviour
{
    public  List<Buff> BuffList;

    private List<Buff> DisposeList;
    public Entity ent;

    public WeaponDataChanger weaponChanger;
    public EntityDataChanger entityChanger;

    public void Init()
    {
        BuffList = new List<Buff>();
        DisposeList = new List<Buff>();
        weaponChanger = new WeaponDataChanger();
        entityChanger = new EntityDataChanger();
        weaponChanger = new WeaponDataChanger();
        entityChanger = new EntityDataChanger();
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

public class WeaponDataChanger
{
    //变量名以mul结尾表示乘算，plu表示加算
    public int pentrationPlu;
    public float stopPowerMul;
    public float shotSpeedMul;
    public float baseSpreadMul;
    public float fireTimesPlu;
    public float fireCDMul;
    public float rangeMul;
    public WeaponDataChanger()
    {
        pentrationPlu = 0;
        stopPowerMul = 1;
        shotSpeedMul = 1;
        baseSpreadMul = 1; 
        fireTimesPlu = 0;
        fireCDMul = 1;
        rangeMul = 1;

    }
}
public class EntityDataChanger
{
    //变量名以mul结尾表示乘算，plu表示加算
    public float maxHealthMul;
    public float moveSpeedMul;

    public EntityDataChanger()
    {
        maxHealthMul = 1;
        moveSpeedMul = 1;
    }
}
