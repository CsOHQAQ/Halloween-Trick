using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QxFramework.Core;

public class ReminderUI : UIBase
{
    private Text text;
    private float counter = 3f;
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        text = _gos["Text"].GetComponent<Text>();
        text.text = (string)args;
    }
    private void Update()
    {
        counter -= Time.deltaTime;
        if (counter < 0)
        {
            UIManager.Instance.Close("ReminderUI");
        }
    }
    protected override void OnClose()
    {
        base.OnClose();
    }
}
