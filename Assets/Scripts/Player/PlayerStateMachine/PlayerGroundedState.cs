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
        Ctx.EnableFootIK = true;
        if (Ctx.IsInAir)
        {
            Ctx.Animator.SetTrigger("Land");
            Ctx.IsInAir = false;
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
        if(!Ctx.IsIdle && !Ctx.StartVault)
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
        else if(Ctx.StartVault)
        {
            SetSubState(Factory.Vaulting());
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
        if(Ctx.IsClimbing)
        {
            SwitchState(Factory.Climbing());
        }
    }
}
