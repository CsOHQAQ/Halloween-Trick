using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerMoveState", menuName = "State/Player/PlayerMoveState")]
public class PlayerMoveState : MoveState
{
    public override void OnEnterState(State LastState)
    {
        base.OnEnterState(LastState);
    }
    public override void Update()
    {
        base.Update();
        Vector2 move = new Vector2();
        #region 移动的按键检测，这个事玩家尚且拥有自己的自主意识时留下的产物
        /*
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
        if (InputManager.Instance.GetButton(InputEnum.Run))
        {
            Debug.Log("Start Running"+manager.human.data);
            manager.human.Run();
        }

        if (manager.human.weaponManager.CurWeapon.data.Method == FireMethod.Auto)
        {
            if (InputManager.Instance.GetButton(InputEnum.Fire))
            {
               
                if (manager.human.Fire(GameWatcher.Instance.CurSituationManager.aim.CurrentSpread))
                    GameWatcher.Instance.CurSituationManager.aim.AimSpread(manager.human.data.Accuracy * 5);

            }
        }
        else
        {
            if (InputManager.Instance.GetButtonDown(InputEnum.Fire))
            {
                if (manager.human.Fire(GameWatcher.Instance.CurSituationManager.aim.CurrentSpread))
                    GameWatcher.Instance.CurSituationManager.aim.AimSpread(manager.human.data.Accuracy * 5);
            }
        }

        if (!manager.human.isMoveX && !manager.human.isMoveY)
            manager.ChangeState<PlayerStandState>();
        else
        {
            //Debug.Log(move);
            manager.human.Move(move);
        }
        */
        #endregion

        //ReloadDetect();
        CommandDetect();
    }
    public override void OnExitState()
    {
        base.OnExitState();
    }
    private void ReloadDetect()
    {
        if (InputManager.Instance.GetButton(InputEnum.Reload))
        {
            manager.ChangeState<PlayerReloadState>();
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
