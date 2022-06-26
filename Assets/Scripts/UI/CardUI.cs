using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using QxFramework.Core;

public class CardUI : UIBase
{
    public CardBase card;
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        var c = (CardBase)args;
        card = new CardBase(c.ID);
        Refresh();
    }

    protected override void OnClose()
    {
        base.OnClose();
    }
    public void Refresh()
    {
        Debug.Log(ResourceManager.Instance.Load<Sprite>(card.imgPath));
        _gos["BuffImg"].GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(card.imgPath);
        _gos["LevelText"].GetComponent<Text>().text = (card.ID % 10).ToString();
    }
}
