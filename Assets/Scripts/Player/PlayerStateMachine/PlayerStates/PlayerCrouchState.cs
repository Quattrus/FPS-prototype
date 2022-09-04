using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class PlayerCrouchState : PlayerBaseState
{
    public PlayerCrouchState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory)
    {

    }
    public override void EnterState()
    {
        Ctx.Speed = Ctx.CrouchSpeed;

    }
    public override void UpdateState()
    {
        CheckSwitchStates();
        CrouchAnim();
    }
    public override void ExitState()
    {
        
    }
    public override void InitializeSubstate()
    {
        
    }
    public override void CheckSwitchStates()
    {
        if(Ctx.IsIdle && !Ctx.IsCrouching)
        {
            SwitchState(Factory.Idle());
        }
        else if(!Ctx.IsIdle && !Ctx.IsCrouching)
        {
            SwitchState(Factory.Walk());
        }
        
    }

    private void CrouchAnim()
    {
        Ctx.Animator.SetFloat("MoveXCrouch", Ctx.CurrentAnimationBlendVector.x);
        Ctx.Animator.SetFloat("MoveZCrouch", Ctx.CurrentAnimationBlendVector.y);
    }
}
