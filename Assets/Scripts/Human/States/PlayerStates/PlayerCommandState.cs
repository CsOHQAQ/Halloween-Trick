using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using QxFramework.Core;
[CreateAssetMenu(fileName = "PlayerCommandState", menuName = "State/Player/PlayerCommandState")]
public class PlayerCommandState : State
{
    public TeamMateBase curChoose;
    public CMDCreator cmdCreator;
    public override void Init()
    {
        base.Init();
        cmdCreator = new CMDCreator();

    }
    public override void OnEnterState(State LastState)
    {
        base.OnEnterState(LastState);
        RefreshCmd();
        MessageManager.Instance.Get<PlayerControl.PlayerMessage>().DispatchMessage(PlayerControl.PlayerMessage.EnterCommand, manager.human);
        UIManager.Instance.Open("CMDHintUI",1);
        cmdCreator.Init();
        
    }
    public override void Update()
    {
        base.Update();
        cmdCreator.update();

        if (InputManager.Instance.GetButtonDown(InputEnum.Command))
        {
            manager.ChangeState<PlayerStandState>();
        }
    }
    public override void OnExitState()
    {
        base.OnExitState();
        MessageManager.Instance.Get<PlayerControl.PlayerMessage>().DispatchMessage(PlayerControl.PlayerMessage.LeaveCommand, manager.human);
        cmdCreator.Close();
        UIManager.Instance.Close("CMDHintUI");
    }

    private void RefreshCmd()
    {
        curChoose = null;
    }
}
