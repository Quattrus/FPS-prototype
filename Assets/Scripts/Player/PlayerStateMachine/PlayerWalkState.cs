using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {

    }
    public override void EnterState()
    {
        Ctx.Speed = Ctx.WalkSpeed;
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
        if(Ctx.IsIdle)
        {
            SwitchState(Factory.Idle());
        }
        else if(!Ctx.IsIdle && Ctx.IsSprinting)
        {
            SwitchState(Factory.Run());
        }
        else if(!Ctx.IsIdle && !Ctx.IsSprinting && !Ctx.Jumped && Ctx.PlayerVelocityY < -10f)
        {
            SwitchState(Factory.Falling());
        }
    }
}
