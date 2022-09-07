using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory)
    {
        InitializeSubstate();
        IsRootState = true;
    }
    public override void EnterState()
    {
        Ctx.PlayerVelocityY = Ctx.PlayerVelocityY * Time.deltaTime;
        Ctx.Animator.SetTrigger("DefaultAir");
        Ctx.IsInAir = true;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        Ctx.IsGrounded = true;
        Ctx.IsFalling = false;
    }
    public override void InitializeSubstate()
    {
        
    }
    public override void CheckSwitchStates()
    {
        if(Ctx.IsGrounded)
        {
            SwitchState(Factory.Grounded());
        }
    }
}
