using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {

    }
    public override void EnterState()
    {
        
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
        if(!Ctx.IsIdle)
        {
            SwitchState(Factory.Walk());
        }
        else if(!Ctx.IsIdle && Ctx.IsSprinting)
        {
            SwitchState(Factory.Run());
        }
        else if(Ctx.IsIdle && Ctx.PlayerVelocityY < -10f)
        {
            SwitchState(Factory.Falling());
        }
        else if (Ctx.IsCrouching)
        {
            SwitchState(Factory.Crouching());
        }
    }
}
