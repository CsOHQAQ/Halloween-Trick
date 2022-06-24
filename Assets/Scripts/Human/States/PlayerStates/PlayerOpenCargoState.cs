using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
[CreateAssetMenu(fileName = "PlayerOpenCargoState", menuName = "State/Player/PlayerOpenCargoState")]
public class PlayerOpenCargoState : State
{
    private float OpenInterval;
    public  UIBase CargoUI;
    public override void Init()
    {
        base.Init();
    }

    public override void OnEnterState(State LastState)
    {
        base.OnEnterState(LastState);
        if (!GameWatcher.Instance.CurSituationManager.isClear)
            CargoUI = UIManager.Instance.Open("PlayerCargoUI", 2, "", manager.human.data.backpack);
        else
            CargoUI = UIManager.Instance.Open("SurvivePanelUI");
        CameraControl.Instance.StopFollowing();
    }

    public override void Update()
    {
        base.Update();
        if ((InputManager.Instance.GetButtonDown(InputEnum.Bag)||InputManager.Instance.GetButtonDown(InputEnum.Quit))&&OpenInterval<0)
        {
            UIManager.Instance.Close(CargoUI);
            manager.ChangeState<StandState>();
        }

        OpenInterval -= manager.human.time.deltaTime;
    }

    public override void OnExitState()
    {
        base.OnExitState();
        CameraControl.Instance.StartFollowing();
    }
}
