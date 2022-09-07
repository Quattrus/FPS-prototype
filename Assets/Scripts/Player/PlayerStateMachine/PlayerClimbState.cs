using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbState : PlayerBaseState

{
    public PlayerClimbState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        InitializeSubstate();
        IsRootState = true;
    }
    public override void EnterState()
    {

    }
    public override void UpdateState()
    {
        CheckSwitchStates();
        if (Ctx.ClimbTransition)
        {
            MoveTowards();
        }
    }
    public override void ExitState()
    {
        
    }
    public override void InitializeSubstate()
    {
       
    }
    public override void CheckSwitchStates()
    {
        if (!Ctx.IsClimbing && Ctx.IsFalling)
        {
            SwitchState(Factory.Falling());
        }
        if (!Ctx.IsClimbing && Ctx.IsGrounded)
        {
            SwitchState(Factory.Grounded());
        }
    }

    private void MoveTowards()
    {
        Vector3 currentVelocity = new Vector3(Ctx.PlayerVelocityX, Ctx.PlayerVelocityY, Ctx.PlayerVelocityZ);
        Vector3 goTowards = new Vector3(Ctx.transform.position.x, Ctx.transform.position.y, Ctx.LadderBoundsTransformPosition.transform.position.z);
        Ctx.transform.position = Vector3.SmoothDamp(Ctx.transform.position, goTowards, ref currentVelocity, 0.1f);
    }

}
