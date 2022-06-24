using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;
[CreateAssetMenu(fileName = "StandState", menuName = "State/Base/StandState")]
public class StandState : State
{
    public override void Init()
    {
        base.Init();
    }
    public override void Update()
    {
        base.Update();  
        manager.human.SlowDownX();
        manager.human.SlowDownY();

        //if (manager.human.isMoveX || manager.human.isMoveY) //?
        //{
        //    manager.ChangeState<Move_State>();
        //}
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    } 
    public override void OnEnterState(State LastState)
    {
        base.OnEnterState(LastState);
    }
    public override void OnExitState()
    {
        base.OnExitState();
    }
}
