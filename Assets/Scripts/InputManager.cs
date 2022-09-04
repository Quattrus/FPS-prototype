using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Player;

public class InputManager : MonoBehaviour
{
    [Header("Player Inputs")]
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerInput.OnClimbActions onClimb;

    [Header("Player Movements")]
    private PlayerStateMachine playerStateMachine;

    [SerializeField] Gun gun;

    private Coroutine fireCoroutine;


    public PlayerInput.OnFootActions OnFoot { get { return onFoot; } set { onFoot = value; } }
    public PlayerInput.OnClimbActions OnClimb { get { return onClimb; } set { onClimb = value; } }


    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = new PlayerInput().OnFoot;
        onClimb = new PlayerInput().OnClimb;
        playerStateMachine = GetComponent<PlayerStateMachine>();
        onFoot.Jump.performed += ctx => playerStateMachine.Jump();
        onFoot.CrouchStart.started += ctx => playerStateMachine.Crouch();
        onFoot.CrouchEnd.canceled += ctx => playerStateMachine.Crouch();
        onFoot.SprintStart.started += ctx => playerStateMachine.SprintStart();
        onFoot.SprintFinish.canceled += ctx => playerStateMachine.SprintFinish();
        onFoot.AimStart.started += ctx => playerStateMachine.PlayerAimStart();
        onFoot.AimFinish.canceled += ctx => playerStateMachine.PlayerAimFinished();
        onFoot.Reload.performed += ctx => gun.ReloadGun();
        onFoot.Shoot.started += _ => StartFiring();
        onFoot.Shoot.canceled += _ => StopFiring();

    }

    //Tell the PlayerMotor.cs to move using the value given from our movement action.
    void FixedUpdate()
    {

    }
    private void Update()
    {
        if (playerStateMachine.IsClimbing)
        {
            playerStateMachine.ProcessClimb(onClimb.ClimbMovement.ReadValue<Vector2>());
        }
        if(!playerStateMachine.IsClimbing)
        {
            playerStateMachine.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
            playerStateMachine.IdleCheck(onFoot.Movement.ReadValue<Vector2>());
            playerStateMachine.ProcessLook(onFoot.Look.ReadValue<Vector2>());
        }



    }

    private void StartFiring()
    {
        if(gun.gameObject != null)
        {
            fireCoroutine = StartCoroutine(gun.RapidFire());
        }
        
    }
    private void StopFiring()
    {
        if(fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
    }

    private void OnEnable()
    {
        onFoot.Enable();
        onClimb.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
        onClimb.Disable();
    }
}
