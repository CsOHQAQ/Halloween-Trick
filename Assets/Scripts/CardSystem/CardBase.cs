using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBase 
{
    public float cost;
    public string Description;
    public string imgPath;
    public string skill;
    public string Name;
    public int ID;

    public virtual void Use()
    {

    }
    public virtual void Destory()
    {

    }

    /// <summary>
    /// 将ref传进来的card按照这个实例复制一张
    /// </summary>
    /// <param name="card"></param>
    public virtual void Copy<T>(ref T card)where T:CardBase
    {

    }
    
}
