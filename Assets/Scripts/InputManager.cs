using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

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
        onFoot.Crouch.performed += ctx => playerMotor.Crouch();
        onFoot.SprintStart.performed += ctx => playerMotor.SprintStart();
        onFoot.SprintFinish.performed += ctx => playerMotor.SprintFinish();
        onFoot.Shoot.started += _ => StartFiring();
        onFoot.Shoot.canceled += _ => StopFiring();
        onFoot.AimStart.performed += ctx => playerLook.PlayerAimStart();
        onFoot.AimFinish.performed += ctx => playerLook.PlayerAimFinished();
        onFoot.Reload.performed += ctx => gun.ReloadGun();

    }

    //Tell the PlayerMotor.cs to move using the value given from our movement action.
    void FixedUpdate()
    {
        playerMotor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
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
