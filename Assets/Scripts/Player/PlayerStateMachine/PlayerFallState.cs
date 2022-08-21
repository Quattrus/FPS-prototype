using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    private float fallSpeed = 10f;
    public PlayerFallState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory)
    {
        InitializeSubstate();
        IsRootState = true;
        Ctx.Animator.SetBool("DefaultFall", true);
    }
    public override void EnterState()
    {
        Ctx.PlayerVelocityY = Ctx.PlayerVelocityY * fallSpeed * Time.deltaTime;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        Ctx.IsGrounded = true;
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
