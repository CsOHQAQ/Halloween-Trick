using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using QxFramework.Core;
using UnityEngine;
public class ShopUI : UIBase
{
    CardSlotUI curChoose=null;
    int curChoosePos = 0;
    bool curChooseShop = true;
    int deleteCount = 0;

    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        for(int i = 0; i < 6; i++)
        {
            UIBase sellCard = UIManager.Instance.Open("SellCardSlot", 2, "", new CardBase(CardManager.Instance.DrawCardFromAll()));
            sellCard.transform.SetParent(_gos["SellCardList"].transform);
            sellCard.GetComponent<Button>().onClick.AddListener(() =>
            {
                curChooseShop = true;
                curChoosePos = i;
                ChangeHighLight(sellCard.GetComponent<CardSlotUI>());
                ShowInfo(sellCard.GetComponent<CardSlotUI>().cardUI.card);
            });
        }
        foreach(var card in CardManager.Instance.CargoCard)
        {
            UIBase cargoCard = UIManager.Instance.Open("CargoCardSlot", 2,"",card);
            cargoCard.transform.SetParent(_gos["CargoCardList"].transform);
            cargoCard.GetComponent<Button>().onClick.AddListener(() =>
            {
                curChooseShop = false;
                curChoosePos = CardManager.Instance.CargoCard.IndexOf(card);

                ChangeHighLight(cargoCard.GetComponent<CardSlotUI>());
            });
        }

        _gos["ConfirmBtn"].GetComponent<Button>().onClick.AddListener(() =>
        {
            CardSlotUI cardSlot = _gos["SellCardList"].transform.GetChild(curChoosePos).GetComponent<CardSlotUI>();
            CardManager.Instance.CargoCard.Add(cardSlot.cardUI.card);
            CardManager.Instance.CurMoney -= cardSlot.cardUI.card.cost;
            cardSlot.GetComponent<Button>().interactable = false;

        });

        _gos["DeleteBtn"].GetComponent<Button>().onClick.AddListener(() =>
        {
            deleteCount++;
            _gos["DeleteText"].GetComponent<Text>().text = deleteCount.ToString() + "/2";
            CardManager.Instance.CargoCard.RemoveAt(curChoosePos);
            Refresh();
        });
        _gos["ContinueBtn"].GetComponent<Button>().onClick.AddListener(() =>
        {
            ProcedureManager.Instance.ChangeTo<BattleProcedure>();

        });
        _gos["CardInfoText"].GetComponent<Text>().text = null;
    }
    private void Update()
    {
        _gos["MoneyRemain"].GetComponentInChildren<Text>().text = CardManager.Instance.CurMoney.ToString();
        _gos["DeleteBtn"].GetComponent<Button>().interactable = !curChooseShop&&(deleteCount<2);

    }
    public void ChangeHighLight(CardSlotUI next)
    {
        if (curChoose != null)
        {
            curChoose.isSelected = false;
        }
        curChoose = next;
        if(next!=null)
        next.isSelected = true;
    }

    public void ShowInfo(CardBase cardBase)
    {
        _gos["CardInfoText"].GetComponent<Text>().text = cardBase.Name + "\n" + cardBase.Description;
        if (CardManager.Instance.CargoCard.Count < 15 && CardManager.Instance.CurMoney >=cardBase.cost)
        {
            _gos["ConfirmBtn"].GetComponent<Button>().interactable = true;
        }
        else
            _gos["ConfirmBtn"].GetComponent<Button>().interactable = false;
    }
    public void Refresh()
    {
        int n = _gos["CargoCardList"].transform.childCount;
        
        for (int i = 0; i < n; i++)
        {
            Debug.Log(_gos["CargoCardList"].transform.GetChild(0).name);
            DestroyImmediate(_gos["CargoCardList"].transform.GetChild(0).gameObject);
        }

        foreach (var card in CardManager.Instance.CargoCard)
        {
            UIBase cargoCard = UIManager.Instance.Open("CargoCardSlot", 2, "", card);
            cargoCard.transform.SetParent(_gos["CargoCardList"].transform);
            cargoCard.GetComponent<Button>().onClick.AddListener(() =>
            {
                curChooseShop = false;
                curChoosePos = CardManager.Instance.CargoCard.IndexOf(card);

                ChangeHighLight(cargoCard.GetComponent<CardSlotUI>());
            });
        }

    }

}
