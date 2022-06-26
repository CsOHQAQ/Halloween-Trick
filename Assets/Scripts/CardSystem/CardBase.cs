using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

public class CardBase 
{
    public float cost;
    public string Description;
    public string imgPath;
    public string skill;
    public string Name;
    public int ID;
    public void Init(int iD)
    {
        TableAgent tab = new TableAgent();
        ID = iD;

        tab.Add(ResourceManager.Instance.Load<TextAsset>("Text/Table/Card").text);
        cost = tab.GetFloat("Card", ID.ToString(), "Cost");
        Description= tab.GetString("Card", ID.ToString(), "Description");
        imgPath = tab.GetString("Card", ID.ToString(), "ImgPath");
        skill = tab.GetString("Card", ID.ToString(), "Skill");
        Name = tab.GetString("Card", ID.ToString(), "Name");
    }
    public CardBase(int id)
    {
        Init(id);
    }
}
