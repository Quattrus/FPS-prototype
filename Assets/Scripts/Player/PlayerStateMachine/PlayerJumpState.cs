using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{

    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory)
    {
        InitializeSubstate();
        IsRootState = true;
        Ctx.Animator.SetBool("DefaultJump", true);
    }
    public override void EnterState()
    {
        Jump();
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        Ctx.IsFalling = true;
    }
    public override void InitializeSubstate()
    {
       
    }
    public override void CheckSwitchStates()
    {
       if(Ctx.IsFalling)
        {
            SwitchState(Factory.Falling());
        }
    }
    private void Jump()
    {
        if (Ctx.IsSprinting)
        {
            Ctx.Animator.CrossFade(Ctx.JumpAnimationSprint, Ctx.AnimationPlayTransition);
        }
        else if (!Ctx.IsSprinting)
        {
            Ctx.Animator.CrossFade(Ctx.JumpAnimationIdle, Ctx.AnimationPlayTransition);
        }
        Ctx.PlayerVelocityY = Mathf.Sqrt(Ctx.JumpHeight * -3.0f * Ctx.Gravity);
    }
}
