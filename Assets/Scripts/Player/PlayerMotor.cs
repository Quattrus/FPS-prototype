using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    [Header("Initialization")]
    private CharacterController characterController;
    private Vector3 playerVelocity;
    //animations
    private Animator animator;
    private int moveXAnimationParameterID;
    private int moveZAnimationParameterID;
    private int jumpAnimationIdle;
    private int jumpAnimationSprint;
    [SerializeField] float animationPlayTransition = 0.15f;
    Vector3 moveDirection = Vector3.zero;

    //SmoothDamp
    private Vector2 currentAnimationBlendVector;
    Vector2 animationVelocity;
    [SerializeField] float animationSmoothTime = 0.1f;


    [Header("Player Movement Checks")]
    [SerializeField] private bool isSprinting;
    [SerializeField] private bool canSprint;
    [SerializeField] private bool isSprinted;
    [SerializeField] bool isGrounded;
    [SerializeField] bool jumped;
    private bool isCrouching = false;
    private bool lerpCrouch = false;
    [SerializeField] float distanceToGround;
    

    [Header("Player Movement Variables")]
    [SerializeField] float gravity = -9.8f;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float crouchTimer = 5f;

    [Header("Movement Speed")]
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float crouchSpeed = 2f;

    void Awake()
    {
        ///<summary>
        ///These are all movement related.
        /// </summary>
        characterController = GetComponent<CharacterController>();
        distanceToGround = GetComponent<CharacterController>().bounds.extents.y;
        canSprint = true;
        speed = walkSpeed;


        //animations
        animator = GetComponent<Animator>();
        moveXAnimationParameterID = Animator.StringToHash("MoveX");
        moveZAnimationParameterID = Animator.StringToHash("MoveY");
        jumpAnimationIdle = Animator.StringToHash("JumpIdle");
        jumpAnimationSprint = Animator.StringToHash("JumpSprint");

        //animator.SetFloat(moveXAnimationParameterID, 1f);

    }

    private void FixedUpdate()
    {
        GroundCheck();
    }
    void Update()
    {
        ///<summary>
        ///These are all movement related
        /// </summary>
        
        CrouchFunctionality();
        CrouchSprintCheck();

    }


    /// <summary>
    /// These are all movement related
    /// </summary>
    //this receives the inputs from the InputManager.cs and applies it to the character controller
    public void ProcessMove(Vector2 input)
    {
       
        if (isSprinting)
        {
            input.x += Mathf.Lerp(input.x, 1,  1 * Time.deltaTime);
            input.y += Mathf.Lerp(input.y, 1, 1 * Time.deltaTime);
        }
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        playerVelocity.y += gravity * Time.fixedDeltaTime;
        characterController.Move(playerVelocity * Time.fixedDeltaTime);

        
            //smoothdamp
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref animationVelocity, animationSmoothTime);
        moveDirection.x = currentAnimationBlendVector.x;
        moveDirection.z = currentAnimationBlendVector.y;
        characterController.Move(transform.TransformDirection(moveDirection) * speed * Time.fixedDeltaTime);
        animator.SetFloat(moveXAnimationParameterID, currentAnimationBlendVector.x);
        animator.SetFloat(moveZAnimationParameterID, currentAnimationBlendVector.y);

    }



    public void Jump()
    {
        if(isGrounded && !isCrouching)
        {
            if(isSprinting)
            {
                animator.CrossFade(jumpAnimationSprint, animationPlayTransition);
            }
            else if(!isSprinting)
            {
                animator.CrossFade(jumpAnimationIdle, animationPlayTransition);
            }
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

    private bool GroundCheck()
    {
         //isGrounded = characterController.isGrounded;
      isGrounded = Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.46f);
        return isGrounded;
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
            
            isSprinting = true;
            speed = sprintSpeed;
            StartCoroutine(SprintDuration());
            isSprinted = true;
            canSprint = false;
            
        }

    }
    public void SprintFinish()
    {
        isSprinting = false;
        if (!isCrouching)
        {     
            speed = walkSpeed;
        }
    }

    IEnumerator SprintDuration()
    {
        yield return new WaitForSeconds(3);
        CrouchCheck();
        isSprinting = false;
        yield return new WaitForSeconds(3);
        isSprinted = false;
        canSprint = true;
    }
///<summary>
///end of movement codes.
/// </summary>


}
