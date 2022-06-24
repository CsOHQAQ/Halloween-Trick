using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Tasks.Actions;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using QxFramework.Core;
using UnityEngine.UI;

[Category("Custom")]
[Description("装填弹药")]
public class Reload : ActionTask
{
    private EnemyBase self;
    private float ReloadTime;
    private float curCount;
    private Transform ReloadUI;
    private UIBase UI;

    protected override string info
    {
        get { return $"装填弹药"; }
    }

    protected override void OnExecute()
    {
        self = agent.GetComponent<EnemyBase>();
        ReloadTime = self.weaponManager.CurWeapon.data.ReloadTime / self.data.ReloadSpeed;
        curCount = 0; //这个东西会继承上一次的值
        ReloadUI = self.GetComponent<Transform>().Find("UIOpener");
        UI = ReloadUI.GetComponent<UIOpener>().OpenReloadUI();
        Debug.Log("#State" + self.name + "开始换弹,当前弹药量为" + self.weaponManager.CurWeapon.data.CurAmmo);
        self.BTBLock = BTBlockEnum.OnBlock;
    }

    protected override void OnUpdate()
    {
        curCount += self.time.deltaTime;
        ReloadUI.GetComponent<UIOpener>().ReloadUpdate();//刷新UI位置
        UI.GetComponent<ReloadUI>().Animation(ReloadTime);//刷新UI动画

        if (curCount >= ReloadTime)
        {
            self.weaponManager.CurWeapon.Reload();
            Debug.Log("#State" + self.name + "完成换弹,当前弹药量为" + self.weaponManager.CurWeapon.data.CurAmmo);
            ReloadUI.GetComponent<UIOpener>().DestroyReloadUI();
            UI.GetComponent<ReloadUI>().Fill.GetComponent<Image>().fillAmount = 0;
            UI.GetComponent<ReloadUI>().FillOfBullet.GetComponent<Image>().fillAmount = 0;
            self.BTBLock = BTBlockEnum.OnChange;
            EndAction(true);
        }
    }

}