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
    private PlayerMotor playerMotor;
    private PlayerLook playerLook;

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
        playerMotor = GetComponent<PlayerMotor>();
        playerLook = GetComponent<PlayerLook>();
        onFoot.Jump.performed += ctx => playerMotor.Jump();
        onFoot.CrouchStart.started += ctx => playerMotor.Crouch();
        onFoot.CrouchEnd.canceled += ctx => playerMotor.Crouch();
        onFoot.SprintStart.started += ctx => playerMotor.SprintStart();
        onFoot.SprintFinish.canceled += ctx => playerMotor.SprintFinish();
        onFoot.Shoot.started += _ => StartFiring();
        onFoot.Shoot.canceled += _ => StopFiring();
        onFoot.AimStart.started += ctx => playerLook.PlayerAimStart();
        onFoot.AimFinish.canceled += ctx => playerLook.PlayerAimFinished();
        onFoot.Reload.performed += ctx => gun.ReloadGun();

    }

    //Tell the PlayerMotor.cs to move using the value given from our movement action.
    void FixedUpdate()
    {
        playerMotor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
        playerLook.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void StartFiring()
    {
        fireCoroutine = StartCoroutine(gun.RapidFire());
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
