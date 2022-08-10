using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    [Header("Initialization")]
    private Rigidbody playerRb;

    //private CharacterController characterController;
    //private Vector3 playerVelocity;


    [Header("Player Movement Checks")]
    [SerializeField] private bool canJump = true;
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask groundLayer;
    private bool isGrounded;


    //[SerializeField] private bool isSprinting;
    //[SerializeField] private bool canSprint;
    //[SerializeField] private bool isSprinted;
    //private bool isCrouching = false;
    //private bool lerpCrouch = false;

    [Header("Player Movement Variables")]
    [SerializeField] Transform orientation;
    private Vector3 moveDirection = Vector3.zero;
    [SerializeField] float jumpForce = 10;
    [SerializeField] float jumpCooldown = 2f;
    [SerializeField] float airMultiplier;
    [SerializeField] float groundFriction;
    [SerializeField] float speed = 5f;

    //[SerializeField] float gravity = -30f; //default is -9.8f;
    //[SerializeField] float jumpHeight = 3f;
    //[SerializeField] float crouchTimer = 5f;

    //[Header("Movement Speed")]
    //[SerializeField] float sprintSpeed = 8f;
    //[SerializeField] float walkSpeed = 5f;
    //[SerializeField] float crouchSpeed = 2f;

    void Start()
    {
        ///<summary>
        ///These are all movement related.
        /// </summary>
        playerRb = GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
        //canSprint = true;
        //speed = walkSpeed;
    }


    void Update()
    {
        ///<summary>
        ///These are all movement related
        /// </summary>
        GroundCheck();
        CrouchFunctionality();
        CrouchSprintCheck();
        VelocityLimit();
    }


    /// <summary>
    /// These are all movement related
    /// </summary>
    //this receives the inputs from the InputManager.cs and applies it to the character controller
    public void ProcessMove(Vector2 input)
    {
        //Vector3 moveDirection = Vector3.zero;
        //moveDirection.x = input.x;
        // moveDirection.z = input.y;
        //characterController.Move(transform.TransformDirection(moveDirection) * speed * Time.fixedDeltaTime);
        // if(isGrounded && playerVelocity.y < 0)
        // {
        //   playerVelocity.y = -2f;
        //}
        //playerVelocity.y += gravity * Time.fixedDeltaTime;
        //  characterController.Move(playerVelocity * Time.fixedDeltaTime);


        //Grabs the moveDirection and applies it to orientation's position based on input's values.
        if(isGrounded)
        {
            moveDirection = orientation.forward * input.y + orientation.right * input.x;
            playerRb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            playerRb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);
        }
        

        //handles drag
        if (isGrounded)
        {
            playerRb.drag = groundFriction;
        }
        else
        {
            playerRb.drag = 0;
        }
    }

    private void VelocityLimit()
    {
        Vector3 flatVelocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);

        //limit velocity if needed
        if(flatVelocity.magnitude > speed)
        {
            Vector3 maxVelocity = flatVelocity.normalized * speed;
            playerRb.velocity = new Vector3(maxVelocity.x, playerRb.velocity.y, maxVelocity.z);
        }
    }

    public void Jump()
    {
        //if(isGrounded && !isCrouching)
        // {
        //    playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        //  }

        //reset velocity.y

        if(canJump && isGrounded)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);
            canJump = false;
            playerRb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        canJump = true;
    }

    public void Crouch()
    {
        //isCrouching = !isCrouching;
       // crouchTimer = 0;
      //  lerpCrouch = true;
      //  if (isCrouching)
      //  {
      //      speed = crouchSpeed;
      //  }
       // else
      //  {
       //     speed = walkSpeed;
       // }
    }

    private void GroundCheck()
    {
        // isGrounded = characterController.isGrounded;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
    }

    private void CrouchSprintCheck()
    {
        //if(isCrouching)
       // {
       //     isSprinting = false;
        //    canSprint = false;
       // }
      //  else if(!isCrouching && !isSprinted)
       // {
        //    canSprint = true;
      //  }
      //  else
      //  {
      //      canSprint = false;
      //  }
    }

    private void CrouchCheck()
    {
       // if (!isCrouching)
      //  {
      //      speed = walkSpeed;
      //  }
    }

    private void CrouchFunctionality()
    {
        //slows down crouch speed to make it more realistic
       // if (lerpCrouch)
       // {
           // crouchTimer += Time.deltaTime;
          //  float crouchLerpValue = crouchTimer / 1;
         //   crouchLerpValue *= crouchLerpValue;
         //   if (isCrouching)
         //   {
        //        characterController.height = Mathf.Lerp(characterController.height, 1, crouchLerpValue);
        //    }
         //   else
           // {
              //  characterController.height = Mathf.Lerp(characterController.height, 2, crouchLerpValue);
          //  }
          //  if (crouchLerpValue > 1)
           // {
            //    lerpCrouch = false;
            //    crouchTimer = 0f;
           // }
       // }
    }

    public void SprintStart()
    {    
       //// if (!isCrouching && canSprint && !isCrouching)
       //// {
       //     isSprinting = !isSprinting;
       //     speed = sprintSpeed;
       //     StartCoroutine(SprintDuration());
       //     isSprinted = true;
       //     canSprint = false;
       // }

    }
    public void SprintFinish()
    {
        //isSprinting = !isSprinting;
        //if (!isCrouching)
        //{          
        //    speed = walkSpeed;
        //}
    }

    IEnumerator SprintDuration()
    {
        return null;
        //yield return new WaitForSeconds(3);
        //CrouchCheck();
        //yield return new WaitForSeconds(3);
        //isSprinted = false;
        //canSprint = true;
    }
///<summary>
///end of movement codes.
/// </summary>


}
