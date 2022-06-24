using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

[CreateAssetMenu(fileName = "PlayerStandState",menuName = "State/Player/PlayerStandState")]
public class PlayerStandState : StandState
{
    public override void OnEnterState(State LastState)
    {
        base.OnEnterState(LastState);
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }
    public override void Update()
    {
        base.Update();
        /*
        manager.human.SlowDownX();
        manager.human.SlowDownY();

        
        MoveDetect();
        FireDetect();
        FirstAidDetect();
        ReloadDetect();
        
        */
        OpenBackPackDetect();
        CommandDetect();
    }

    private void MoveDetect()
    {
        if (InputManager.Instance.GetButton(InputEnum.Right) ||
            InputManager.Instance.GetButton(InputEnum.Left) ||
            InputManager.Instance.GetButton(InputEnum.Up) ||
            InputManager.Instance.GetButton(InputEnum.Down))
        {
            manager.ChangeState<PlayerMoveState>();
        }
    }
    private void FireDetect()
    {
        if (manager.human.weaponManager.CurWeapon.data.Method == FireMethod.Auto)
        {
            if (InputManager.Instance.GetButton(InputEnum.Fire))
            {

            }
        }
        else
        {
            if (InputManager.Instance.GetButtonDown(InputEnum.Fire))
            {
            }
        }
    }
    private void FirstAidDetect()
    {
        if (InputManager.Instance.GetButton(InputEnum.Heal))
        {
            manager.ChangeState<PlayerFirstAidState>();
        }
    }
    private void ReloadDetect()
    {
        if (InputManager.Instance.GetButton(InputEnum.Reload))
        {
            manager.ChangeState<PlayerReloadState>();
        }
    }
    private void OpenBackPackDetect()
    {
        if (InputManager.Instance.GetButtonDown(InputEnum.Bag))
        {
            manager.ChangeState<PlayerOpenCargoState>();
        }
    }
    private void CommandDetect()
    {
        if (InputManager.Instance.GetButtonDown(InputEnum.Command))
        {
            manager.ChangeState<PlayerCommandState>();
        }
    }
}
