using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class Buff 
{
    public float LastingTime;//持续时间
    public float Count;//积累槽
    protected float maxCount;//上限
    public float MaxCount
    {
        get
        {
            return maxCount;
        }
    }

    protected BuffManager buffManager;

    public void SetManager(BuffManager bm)
    {
        buffManager = bm;
    }

    /// <summary>
    /// 初始化buff
    /// </summary>
    public virtual void Init()
    {
        Debug.Log("#Buff" + this.ToString());
        InitDataChange();
    }
    /// <summary>
    /// 每帧刷新buff
    /// </summary>
    public virtual void Refresh()
    {
        LastingTime -= Time.deltaTime;
    }
    /// <summary>
    /// 当积累槽积满后触发效果
    /// </summary>
    public virtual void ActivateWhenFull(){}
    /// <summary>
    /// 移除该buff前执行的操作
    /// </summary>
    public virtual void BeforeRemove()
    {
        ClearEffect();
    }
    /// <summary>
    /// 清除该buff效果
    /// </summary>
    public virtual void ClearEffect()
    {
        CancelDataChange();
    }
    /// <summary>
    /// 使用读表进行DataChanger的修改
    /// </summary>
    public virtual void InitDataChange()
    {
    }

    /// <summary>
    /// 清除该buff对DataChange的影响
    /// </summary>
    public virtual void CancelDataChange()
    {
    }
}
