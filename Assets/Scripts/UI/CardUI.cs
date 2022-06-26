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
        if ((int)(card.ID / 100) == 11)
        {
            _gos["BgImg"].GetComponent<Image>().sprite = ResourceManager.Instance.Load < Sprite > (card.imgPath);
            _gos["BuffImg"].GetComponent<Image>().sprite = null;
            _gos["TextBGImg"].SetActive(false);
            _gos["LevelText"].SetActive(false);
        }
        else
        {
            _gos["TextBGImg"].SetActive(true);
            _gos["LevelText"].SetActive(true);
            _gos["BuffImg"].GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(card.imgPath);
            _gos["LevelText"].GetComponent<Text>().text = (card.ID % 10).ToString();
        }
    }
}
