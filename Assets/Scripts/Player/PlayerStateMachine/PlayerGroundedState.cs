using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        InitializeSubstate();
        IsRootState = true;
    }
    public override void EnterState()
    {
        Ctx.Jumped = false;
        Ctx.IsFalling = false;
        if (Ctx.PlayerVelocityY < 0)
        {
            Ctx.PlayerVelocityY = -2f;
        }
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
        if(!Ctx.IsIdle)
        {
            SetSubState(Factory.Walk());
        }
        else if (Ctx.IsIdle)
        {
            SetSubState(Factory.Idle());
        }
        else if(!Ctx.IsIdle && Ctx.IsSprinting)
        {
            SetSubState(Factory.Run());
        }
    }
    public override void CheckSwitchStates()
    {
        //if player is grounded and jump is pressed, switch to jump
        if(Ctx.Jumped)
        {
            SwitchState(Factory.Jump());
        }
        if(Ctx.IsFalling)
        {
            SwitchState(Factory.Falling());
        }
    }
}
