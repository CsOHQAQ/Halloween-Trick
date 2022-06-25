using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Common;
using QxFramework.Utilities;
using System;
using System.Reflection;
public class CardManager : Singleton<CardManager>
{
    public  List<CardBase> CargoCard;
    public List<CardBase> HandCard;
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
