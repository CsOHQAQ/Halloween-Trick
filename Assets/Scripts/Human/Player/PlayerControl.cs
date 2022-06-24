using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;
using QxFramework.Core;
public class PlayerControl : TeamMateBase
{

    public override void Init(string name="")
    {
        base.Init(name); 
    }

    public override void Update()
    {
        base.Update();
        //if (InputManager.Instance.GetButtonDown(InputEnum.Command))
        //{
        //    if (stateManager.currentState is PlayerCommandState)
        //    {
        //        stateManager.ChangeState<PlayerStandState>();
        //    }
        //    else
        //    {
        //        stateManager.ChangeState<PlayerCommandState>();

        //    }
        //}
    }
    public override void LookingAt(Vector2 t)
    {
        if (t != null)
        {
            float angle = Vector2.Angle(Vector2.right, t - (Vector2)transform.position) * (t.y < transform.position.y ? -1 : 1)-90;
            transform.rotation = Quaternion.Euler(0,0,angle);
        }
    }
    private void OnDestroy()
    {

    }
    public enum PlayerMessage
    {
        EnterCommand,
        LeaveCommand,
    }

}
