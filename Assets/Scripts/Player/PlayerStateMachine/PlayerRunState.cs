using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {

    }
    public override void EnterState()
    {
        Ctx.Speed = Ctx.SprintSpeed;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {

    }
    public override void InitializeSubstate()
    {

    }
    public override void CheckSwitchStates()
    {
        if(!Ctx.IsSprinting && Ctx.IsIdle)
        {
            SwitchState(Factory.Idle());
        }
        else if (!Ctx.IsSprinting && !Ctx.IsIdle)
        {
            SwitchState(Factory.Walk());
        }
    }
}
