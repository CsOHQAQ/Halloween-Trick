using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QxFramework.Core;
public class CardSlotUI : UIBase
{
    public bool isSelected=false;
    public CardUI cardUI;

    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        CardBase c = args as CardBase;
        if(_gos.ContainsKey("CostText"))
        _gos["CostText"].GetComponent<Text>().text = c.cost + "G";
        cardUI = UIManager.Instance.Open("CardUI", 2, "", args).GetComponent<CardUI>();
        cardUI.transform.SetParent(_gos["AnPos"].transform);
    }

    private void Update()
    {
        _gos["HighLight"].SetActive(isSelected);


    }
}
