using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{

    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory)
    {
        InitializeSubstate();
        IsRootState = true;
       
    }
    public override void EnterState()
    {
        Ctx.EnableFootIK = false;
        if(!Ctx.IsSprinting)
        {
            Ctx.Animator.SetTrigger("DefaultJump");
        }
        else if(Ctx.IsSprinting)
        {
            if(Ctx.Animator.GetFloat("RightFootCurve") > Ctx.Animator.GetFloat("LeftFootCurve"))
            {
                Ctx.Animator.SetTrigger("SprintJump");
            }
            else if(Ctx.Animator.GetFloat("RightFootCurve") < Ctx.Animator.GetFloat("LeftFootCurve"))
            {
                Ctx.Animator.SetTrigger("SprintJumpMirrored");
            }   
        }

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
      Ctx.PlayerVelocityY = Mathf.Sqrt(Ctx.JumpHeight * -3.0f * Ctx.Gravity);
    }
}
