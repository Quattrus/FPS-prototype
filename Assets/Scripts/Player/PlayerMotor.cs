using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    [Header("Initialization")]
    private CharacterController characterController;
    private Vector3 playerVelocity;


    [Header("Player Movement Checks")]
    [SerializeField] private bool isSprinting;
    [SerializeField] private bool canSprint;
    [SerializeField] private bool isSprinted;
    private bool isGrounded;
    private bool isCrouching = false;
    private bool lerpCrouch = false;

    [Header("Player Movement Variables")]
    [SerializeField] float gravity = -30f; //default is -9.8f;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float crouchTimer = 5f;

    [Header("Movement Speed")]
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float crouchSpeed = 2f;

    void Start()
    {
        ///<summary>
        ///These are all movement related.
        /// </summary>
        characterController = GetComponent<CharacterController>();
        canSprint = true;
        speed = walkSpeed;
    }


    void Update()
    {
        ///<summary>
        ///These are all movement related
        /// </summary>
        GroundCheck();
        CrouchFunctionality();
        CrouchSprintCheck();
    }


    /// <summary>
    /// These are all movement related
    /// </summary>
    //this receives the inputs from the InputManager.cs and applies it to the character controller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        characterController.Move(transform.TransformDirection(moveDirection) * speed * Time.fixedDeltaTime);
        if(isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        playerVelocity.y += gravity * Time.fixedDeltaTime;
        characterController.Move(playerVelocity * Time.fixedDeltaTime);
    }

    public void Jump()
    {
        if(isGrounded && !isCrouching)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void Crouch()
    {
        isCrouching = !isCrouching;
        crouchTimer = 0;
        lerpCrouch = true;
        if (isCrouching)
        {
            speed = crouchSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
    }

    private void GroundCheck()
    {
        isGrounded = characterController.isGrounded;
    }

    private void CrouchSprintCheck()
    {
        if(isCrouching)
        {
            isSprinting = false;
            canSprint = false;
        }
        else if(!isCrouching && !isSprinted)
        {
            canSprint = true;
        }
        else
        {
            canSprint = false;
        }
    }

    private void CrouchCheck()
    {
        if (!isCrouching)
        {
            speed = walkSpeed;
        }
    }

    private void CrouchFunctionality()
    {
        //slows down crouch speed to make it more realistic
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float crouchLerpValue = crouchTimer / 1;
            crouchLerpValue *= crouchLerpValue;
            if (isCrouching)
            {
                characterController.height = Mathf.Lerp(characterController.height, 1, crouchLerpValue);
            }
            else
            {
                characterController.height = Mathf.Lerp(characterController.height, 2, crouchLerpValue);
            }
            if (crouchLerpValue > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }

    public void SprintStart()
    {    
        if (!isCrouching && canSprint && !isCrouching)
        {
            isSprinting = !isSprinting;
            speed = sprintSpeed;
            StartCoroutine(SprintDuration());
            isSprinted = true;
            canSprint = false;
        }

    }
    public void SprintFinish()
    {
        isSprinting = !isSprinting;
        if (!isCrouching)
        {          
            speed = walkSpeed;
        }
    }

    IEnumerator SprintDuration()
    {
        
        yield return new WaitForSeconds(3);
        CrouchCheck();
        yield return new WaitForSeconds(3);
        isSprinted = false;
        canSprint = true;
    }
///<summary>
///end of movement codes.
/// </summary>


}
