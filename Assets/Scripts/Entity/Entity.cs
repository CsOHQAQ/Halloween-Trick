using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

public class Entity 
{
    public string Name;

    public List<string> character;
    public List<string> translatedChara;
    public string sex;
    public string sexualOrientation;

    public string characterInfo;

    public float timeLimit;
    public float counter;
    //头像地址
    public string head;
    //立绘地址
    public string animation;

    public bool isSpecial = false;

    public List<string> likes;
    public List<string> dislikes;



    public int ID;

    public void TagToInfo(int charaID)
    {
        TableAgent tab = new TableAgent();
        tab.Add(ResourceManager.Instance.Load<TextAsset>("Text/Table/Tags").text);
        foreach(var tag in character)
        {
            string[] description = tab.GetStrings("Tags", tag, "Description");
            int desId = Random.Range(0, description.Length);
            characterInfo += description[desId] + "\n";

        }

    }



}
