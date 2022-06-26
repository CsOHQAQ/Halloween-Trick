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
    public float CurMoney;
    private TableAgent tab;
    /// <summary>
    /// 这个是最初的初始化，只执行一次
    /// </summary>
    public void Initialize()
    {
        tab = new TableAgent();
        tab.Add(ResourceManager.Instance.Load<TextAsset>("Text/Table/Card").text);
        CargoCard = new List<CardBase>();
        CurMoney = 0;

        List<int> StartCard = new List<int> { 1001,2001,1001,2001,4001,4001,5001,7001,9001,8001};
        foreach(var id in StartCard)
        {
            CardBase card = new CardBase(id);
            CargoCard.Add(card);
        }
    }
    /// <summary>
    /// 每次重新载入战斗场景的初始化
    /// </summary>
    public void Init()
    {
        HandCard = new List<CardBase>();

    }

    public void DrawCardFromCargo()
    {
        int p = UnityEngine.Random.Range(0, CargoCard.Count);
        CardBase card = new CardBase(CargoCard[p].ID);
        HandCard.Add(card);
        UIBase ui= UIManager.Instance.Open("CardUI", 2, "", card);
        ui.transform.SetParent(UIManager.Instance.GetUI("HandCardSlot").transform);
    }

      public int DrawCardFromAll()
    {
        var idList = tab.CollectKey1("Card");
        int p = UnityEngine.Random.Range(0, idList.Count);
        return int.Parse( idList[p]);
    }


    private void Update()
    {
        if(ProcedureManager.Instance.Current is BattleProcedure)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (HandCard.Count > CurSelect)
                {
                    SkillAffect(HandCard[CurSelect].skill);
                    HandCard.RemoveAt(CurSelect);
                    Transform card= UIManager.Instance.GetUI("HandCardSlot").transform.GetChild(CurSelect);
                    UIManager.Instance.Close(card.GetComponent<UIBase>());
                    Destroy(card.gameObject);
                    if (HandCard.Count <= CurSelect)
                    {
                        CurSelect = Mathf.Max(0,HandCard.Count-1);
                    }
                }
            }
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
            mt.Invoke(null,new object[] {""});
        }
    }
}
