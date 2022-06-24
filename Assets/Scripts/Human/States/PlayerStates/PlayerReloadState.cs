using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
using UnityEngine.UI;   
[CreateAssetMenu(fileName = "PlayerReloadState", menuName = "State/Player/PlayerReloadState")]
public class PlayerReloadState : ReloadState
{
    public override void OnEnterState(State LastState)
    {
        base.OnEnterState(LastState);
    }
    public override void Update()
    {
        base.Update();

        /*
        Vector2 move = new Vector2();
        if (InputManager.Instance.GetButton(InputEnum.Right) && !InputManager.Instance.GetButton(InputEnum.Left))
        {
            move += Vector2.right;
            manager.human.isMoveX = true;
        }
        if (InputManager.Instance.GetButton(InputEnum.Left) && !InputManager.Instance.GetButton(InputEnum.Right))
        {
            move += Vector2.left;
            manager.human.isMoveX = true;
        }
        if (InputManager.Instance.GetButton(InputEnum.Up) && !InputManager.Instance.GetButton(InputEnum.Down))
        {
            move += Vector2.up;
            manager.human.isMoveY = true;
        }
        if (InputManager.Instance.GetButton(InputEnum.Down) && !InputManager.Instance.GetButton(InputEnum.Up))
        {
            move += Vector2.down;
            manager.human.isMoveY = true;
        }
        if (manager.human.isMoveX || manager.human.isMoveY)
        {
            manager.human.Move(move);
        }
        */
    }
    public override void OnExitState()
    {
        base.OnExitState();
    }
}
