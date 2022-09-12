using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Player Inputs")]
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerInput.OnClimbActions onClimb;
    private PlayerInput.OnMeleeActions onMelee;

    [Header("Player Movements")]
    private PlayerStateMachine playerStateMachine;

    [Header("Combat")]
    [SerializeField] GameObject radialWheelBase;

    [SerializeField] Gun gun;
    private Inventory inventory;

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
        inventory = GetComponent<Inventory>();
        playerInput = new PlayerInput();
        onFoot = new PlayerInput().OnFoot;
        onClimb = new PlayerInput().OnClimb;
        onMelee = new PlayerInput().OnMelee;
        playerStateMachine = GetComponent<PlayerStateMachine>();
        onFoot.Jump.performed += ctx => playerStateMachine.Jump();
        onFoot.FlashLight.performed += ctx => gun.Flashlight();
        onFoot.CrouchStart.started += ctx => playerStateMachine.Crouch();
        onFoot.CrouchEnd.canceled += ctx => playerStateMachine.Crouch();
        onFoot.SprintStart.started += ctx => playerStateMachine.SprintStart();
        onFoot.SprintFinish.canceled += ctx => playerStateMachine.SprintFinish();
        onFoot.AimStart.started += ctx => playerStateMachine.PlayerAimStart();
        onFoot.AimFinish.canceled += ctx => playerStateMachine.PlayerAimFinished();
        onFoot.Reload.performed += ctx => gun.ReloadGun();
        onFoot.Shoot.started += _ => StartFiring();
        onFoot.Shoot.canceled += _ => StopFiring();
        onFoot.MainWeapon.performed += ctx => gun.SwitchGuns(1);
        onFoot.SecondaryWeapon.performed += ctx => gun.SwitchGuns(2);
        onMelee.Strike.performed += ctx => radialWheelBase.gameObject.GetComponent<RadialWheelBase>().SelectMeleeStrike();

    }

    private void Update()
    {
        if(!playerStateMachine.IsVaulting && !playerStateMachine.IsClimbing)
        {
            playerStateMachine.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
            playerStateMachine.IdleCheck(onFoot.Movement.ReadValue<Vector2>());
        }
        if(playerStateMachine.IsClimbing)
        {
            playerStateMachine.ProcessClimb(onClimb.ClimbMovement.ReadValue<Vector2>());
        }
        if(playerStateMachine.MeleeMode)
        {
            radialWheelBase.gameObject.GetComponent<RadialWheelBase>().MeleeSelection(onMelee.MeleeSelection.ReadValue<Vector2>());
        }
        else if(!playerStateMachine.MeleeMode)
        {
            playerStateMachine.ProcessLook(onFoot.Look.ReadValue<Vector2>());
        }

    }

    private void StartFiring()
    {
        if(inventory.GunEquipped)
        {
            fireCoroutine = StartCoroutine(gun.RapidFire());
        }
        
    }
    private void StopFiring()
    {
        if(fireCoroutine != null && inventory.GunEquipped)
        {
            StopCoroutine(fireCoroutine);
        }
    }

    private void OnEnable()
    {
        onClimb.Enable();
        onFoot.Enable();
        onMelee.Enable();
    }

    private void OnDisable()
    {
        onClimb.Disable();
        onFoot.Disable();
        onMelee.Disable();
    }
}
