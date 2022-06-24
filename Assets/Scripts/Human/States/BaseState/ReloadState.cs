using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;
using QxFramework.Core;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ReloadState", menuName = "State/Base/ReloadState")]
public class ReloadState : State
{
    public float ReloadTime;//换弹时间会自动读取人物当前装备的武器换弹时间
    private float curCount;
    private Transform ReloadUI;
    private UIBase UI;
    public override void Init()
    {
        base.Init();
    }
    public override void OnEnterState(State LastState)
    {
        base.OnEnterState(LastState);

        if (manager.human.weaponManager.CurWeapon.data.CurAmmo > manager.human.weaponManager.CurWeapon.data.MaxAmmo 
            || GameManager.Get<IItemManager>().GetItemCount(1001, new CargoData[] { manager.human.data.backpack }) == 0)
        {
            Debug.Log("#State无法或没必要换弹,润了");
            manager.ChangeState<StandState>();
        }
        else
        {
            curCount = 0;
            ReloadTime = manager.human.weaponManager.CurWeapon.data.ReloadTime / manager.human.data.ReloadSpeed;
            Debug.Log("#State" + manager.human.name + "开始换弹,当前弹药量为" + manager.human.weaponManager.CurWeapon.data.CurAmmo);
            ReloadUI = manager.human.transform.Find("UIOpener");
            UI = ReloadUI.GetComponent<UIOpener>().OpenReloadUI();
        }
    }
    public override void OnExitState()
    {
        base.OnExitState();
        if (ReloadUI != null)
        {
            ReloadUI.GetComponent<UIOpener>().DestroyReloadUI();
            UI.GetComponent<ReloadUI>().Fill.GetComponent<Image>().fillAmount = 0;
            UI.GetComponent<ReloadUI>().FillOfBullet.GetComponent<Image>().fillAmount = 0;

        }
        
    }
    public override void Update()
    {
        base.Update();
        if(curCount>0)
        manager.human.buffManager.AddBuff(new Buff_Busy(),manager.human.time.deltaTime);

        curCount += manager.human.time.deltaTime;
        ReloadUI.GetComponent<UIOpener>().ReloadUpdate();//刷新UI位置
        UI.GetComponent<ReloadUI>().Animation(ReloadTime);//刷新UI动画
        if (curCount >= ReloadTime)
        {
            ReloadComplete();
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public void ReloadComplete()
    {
        manager.human.Reload();

        Debug.Log("#State" + manager.human.name + "完成换弹,当前弹药量为" + manager.human.weaponManager.CurWeapon.data.CurAmmo);
        manager.ChangeState<StandState>();
    }
}
