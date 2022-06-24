using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Tasks.Actions;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using QxFramework.Core;
using UnityEngine.UI;

[Category("Custom")]
[Description("治疗自己")]
public class FirstAid : ActionTask
{
    private EnemyBase self;
    private float DefaultRec = 3f; //测试 回复3点生命
    private float DefaultRecTime = 3.5f; //测试 3.5s回复时间
    private float RecTime; //真实回复时间
    private float curCount;
    private Transform HealUI;
    private UIBase UI;

    protected override string info
    {
        get { return $"治疗自己"; }
    }

    protected override void OnExecute()
    {
        self = agent.GetComponent<EnemyBase>();
        RecTime = DefaultRecTime / self.data.HealSpeed;
        curCount = 0; //这个东西会继承上一次的值
        HealUI = self.GetComponent<Transform>().Find("UIOpener");
        UI = HealUI.GetComponent<UIOpener>().OpenHealUI();
        self.BTBLock = BTBlockEnum.OnBlock;
    }

    protected override void OnUpdate()
    {
        curCount += self.time.deltaTime;
        HealUI.GetComponent<UIOpener>().HealUpdate();//UI位置刷新
        UI.GetComponent<HealUI>().Animation(RecTime);//UI动画刷新

        if (curCount >= RecTime)
        {
            self.FirstAid(DefaultRec);
            Debug.Log("#State已完成治疗,当前血量为" + self.data.CurrentHealth);
            HealUI.GetComponent<UIOpener>().DestroyHealUI();
            UI.GetComponent<HealUI>().Fill.GetComponent<Image>().fillAmount = 0;
            self.BTBLock = BTBlockEnum.OnChange;
            EndAction(true);
        }
    }

}