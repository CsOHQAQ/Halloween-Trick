using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using QxFramework.Core;

public class TimerUI : UIBase
{
    private Text text;
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        text = _gos["Text"].GetComponent<Text>();

    }
    private void Update()
    {
        GameDateTime timeRemain = GameDateTime.ByMinutes(EntityManager.Instance.MaxTime - EntityManager.Instance.TimeDiff);
        text.text = $"距离夜晚结束还有 {timeRemain.Hours}时{timeRemain.Minutes}分";
    }
    protected override void OnClose()
    {
        base.OnClose();
    }
}
