using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class PlayerVaultState : PlayerBaseState
{

    public PlayerVaultState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory)
    {

    }
    public override void EnterState()
    {
        Ctx.Animator.applyRootMotion = true;
        Ctx.Animator.SetTrigger("VaultMove");
        Ctx.CharacterController.enabled = false;
        Ctx.StartVault = false;
        Ctx.Collider.enabled = true;
        

    }
    public override void UpdateState()
    {if(Ctx.IsVaulting)
        {
            MoveToward();
        }
        
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
        if(Ctx.IsVaulting == false && Ctx.IsIdle)
        {
            Ctx.Animator.applyRootMotion = false;
            SwitchState(Factory.Idle());
        }
        else if(Ctx.IsVaulting == false && !Ctx.IsIdle)
        {
            Ctx.Animator.applyRootMotion = false;
            SwitchState(Factory.Walk());
        }
    }

    private void MoveToward()
    {
        Vector3 currentVelocity = new Vector3(Ctx.PlayerVelocityX, Ctx.PlayerVelocityY, Ctx.PlayerVelocityZ);
        Vector3 targetVaultPosition = new Vector3(Ctx.TargetVaultPosition.x, Ctx.TargetVaultPosition.y + 1, Ctx.TargetVaultPosition.z);
        Ctx.Collider.transform.position = Vector3.SmoothDamp(Ctx.Collider.transform.position, targetVaultPosition, ref currentVelocity, 1f);
    }

}
