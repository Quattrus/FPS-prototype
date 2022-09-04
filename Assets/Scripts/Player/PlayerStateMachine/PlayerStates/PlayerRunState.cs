using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

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
        StaminaControl();
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
            Ctx.Animator.SetTrigger("StopRun");
            SwitchState(Factory.Idle());
        }
        else if (!Ctx.IsSprinting && !Ctx.IsIdle)
        {
            Ctx.Animator.SetTrigger("StopRun");
            SwitchState(Factory.Walk());
        }
        else if(Ctx.IsSprinting && Ctx.PlayerVelocityY < -10f)
        {
            SwitchState(Factory.Falling());
        }
    }

    private void StaminaControl()
    {
        Ctx.StaminaController.Sprinting();
    }

}
