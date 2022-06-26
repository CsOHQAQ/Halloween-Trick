using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Common;
using QxFramework.Utilities;
using System;
using System.Reflection;
using QxFramework.Core;

public class CardManager : MonoSingleton<CardManager>
{
    public  List<CardBase> CargoCard;
    public List<CardBase> HandCard;
    public int CurSelect;
    /// <summary>
    /// 这个是最初的初始化，只执行一次
    /// </summary>
    public void Initialize()
    {
        CargoCard = new List<CardBase>();
    }
    /// <summary>
    /// 每次重新载入战斗场景的初始化
    /// </summary>
    public void Init()
    {
        HandCard = new List<CardBase>();

    }



    
    private void Update()
    {
        if(ProcedureManager.Instance.Current is BattleProcedure)
        {

        }
    }

    private void SkillAffect(string Func)
    {
        if (Func == "")
        {
            return;
        }
        Type t = typeof(CardEffect);//括号中的为所要使用的函数所在的类的类名。

        if (Func.Contains(":"))
        {
            MethodInfo mt = t.GetMethod(Func.Split(':')[0]);
            if (mt == null)
            {
                Debug.Log("No Function: " + Func);
            }
            mt.Invoke(null, new object[] {  Func.Split(':')[1] });
        }
        else
        {
            MethodInfo mt = t.GetMethod(Func);
            if (mt == null)
            {
                Debug.Log("No Function: " + Func);
            }
            mt.Invoke(null, new object[] {""});
        }
    }
}
