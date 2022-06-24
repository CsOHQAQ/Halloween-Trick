using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;
using QxFramework.Core;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "FirstAidState", menuName = "State/Base/FirstAidState")]
public class FirstAidState : State
{
    [Tooltip("治疗所需时间")]
    [HideInInspector]
    public float RecTime=3f;
    [Tooltip("单次恢复的血量")]
    [HideInInspector]
    public float RecHP;

    private float DefaultRec = 3;
    private float DefaultRecTime = 3.5f;
    private float curCount;

    private Transform HealUI;
    private UIBase UI;
    public override void Init()
    {

        base.Init();
        RecHP = DefaultRec;
        
    }
    public override void Update()
    {
        manager.human.SlowDownX();
        manager.human.SlowDownY();
        curCount += manager.human.time.deltaTime;
        HealUI.GetComponent<UIOpener>().HealUpdate();//UI位置刷新
        UI.GetComponent<HealUI>().Animation(RecTime);//UI动画刷新
        if (curCount >= (RecTime))
        {
            HealComplete();
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void OnEnterState(State LastState)
    {
        base.OnEnterState(LastState);
        Debug.Log("#State" + manager.human.name + "进入治疗,当前血量为"+manager.human.data.CurrentHealth);
        curCount = 0;
        RecTime = DefaultRecTime / manager.human.data.HealSpeed;
        HealUI = manager.human.GetComponent<Transform>().Find("UIOpener");
        UI = HealUI.GetComponent<UIOpener>().OpenHealUI();
    }
    public override void OnExitState()
    {
        base.OnExitState();
        RecHP = DefaultRec;
        Debug.Log(HealUI);
        HealUI.GetComponent<UIOpener>().DestroyHealUI();
        UI.GetComponent<HealUI>().Fill.GetComponent<Image>().fillAmount = 0;
        Debug.Log("#State" + manager.human.name + "退出治疗");
    }

    public void HealComplete()
    {
        manager.human.FirstAid(DefaultRec);
        Debug.Log("#State已完成治疗,当前血量为" + manager.human.data.CurrentHealth);
        manager.ChangeState<StandState>();
    }

}
