using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Player Inputs")]
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    [Header("Player Movements")]
    private PlayerLook playerLook;
    private PlayerStateMachine playerStateMachine;

    [SerializeField] Gun gun;

    private Coroutine fireCoroutine;


    public PlayerInput.OnFootActions OnFoot
    {
        get
        {
            return onFoot;
        }
    }

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = new PlayerInput().OnFoot;
        playerLook = GetComponent<PlayerLook>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
        onFoot.Jump.performed += ctx => playerStateMachine.Jump();
        onFoot.CrouchStart.started += ctx => playerStateMachine.Crouch();
        onFoot.CrouchEnd.canceled += ctx => playerStateMachine.Crouch();
        onFoot.SprintStart.started += ctx => playerStateMachine.SprintStart();
        onFoot.SprintFinish.canceled += ctx => playerStateMachine.SprintFinish();
        onFoot.AimStart.started += ctx => playerLook.PlayerAimStart();
        onFoot.AimFinish.canceled += ctx => playerLook.PlayerAimFinished();
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
        if(!playerStateMachine.IsVaulting)
        {
            playerStateMachine.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
            playerLook.ProcessLook(onFoot.Look.ReadValue<Vector2>());
            playerStateMachine.IdleCheck(onFoot.Movement.ReadValue<Vector2>());
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
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
