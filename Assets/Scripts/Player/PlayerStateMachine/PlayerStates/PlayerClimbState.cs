using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class PlayerClimbState : PlayerBaseState

{
    public PlayerClimbState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        
    }
    public override void EnterState()
    {
        Ctx.TransitionClimb = false;
        Ctx.CharacterController.enabled = true;
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
        
    }
}
