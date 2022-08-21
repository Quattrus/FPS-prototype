using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    [Header("Initialization")]
    private CharacterController _characterController;
    private Vector3 _playerVelocity;
    //animations
    private Animator _animator;
    private int _moveXAnimationParameterID;
    private int _moveZAnimationParameterID;
    private int _jumpAnimationIdle;
    private int _jumpAnimationSprint;
    [SerializeField] float _animationPlayTransition = 0.15f;
    Vector3 _moveDirection = Vector3.zero;

    //SmoothDamp
    private Vector2 _currentAnimationBlendVector;
    Vector2 _animationVelocity;
    [SerializeField] float _animationSmoothTime = 0.1f;


    [Header("Player Movement Checks")]
    [SerializeField] private bool _isSprinting;
    [SerializeField] private bool _canSprint;
    [SerializeField] private bool _isSprinted;
    [SerializeField] bool _isGrounded;
    [SerializeField] bool _jumped;
    private bool _isCrouching = false;
    private bool _lerpCrouch = false;
    [SerializeField] float _distanceToGround;
    

    [Header("Player Movement Variables")]
    [SerializeField] float _gravity = -9.8f;
    [SerializeField] float _speed = 5f;
    [SerializeField] float _jumpHeight = 3f;
    [SerializeField] float _crouchTimer = 5f;

    [Header("Movement Speed")]
    [SerializeField] float _sprintSpeed = 8f;
    [SerializeField] float _walkSpeed = 5f;
    [SerializeField] float _crouchSpeed = 2f;

    void Awake()
    {
        ///<summary>
        ///These are all movement related.
        /// </summary>
        _characterController = GetComponent<CharacterController>();
        _distanceToGround = GetComponent<CharacterController>().bounds.extents.y;
        _canSprint = true;
        _speed = _walkSpeed;


        //animations
        _animator = GetComponent<Animator>();
        _moveXAnimationParameterID = Animator.StringToHash("MoveX");
        _moveZAnimationParameterID = Animator.StringToHash("MoveY");
        _jumpAnimationIdle = Animator.StringToHash("JumpIdle");
        _jumpAnimationSprint = Animator.StringToHash("JumpSprint");

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
       
        if (_isSprinting)
        {
            input.x += Mathf.Lerp(input.x, 1,  1 * Time.deltaTime);
            input.y += Mathf.Lerp(input.y, 1, 1 * Time.deltaTime);
        }
        if (_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -2f;
        }
        _playerVelocity.y += _gravity * Time.fixedDeltaTime;
        _characterController.Move(_playerVelocity * Time.fixedDeltaTime);

        
            //smoothdamp
        _currentAnimationBlendVector = Vector2.SmoothDamp(_currentAnimationBlendVector, input, ref _animationVelocity, _animationSmoothTime);
        _moveDirection.x = _currentAnimationBlendVector.x;
        _moveDirection.z = _currentAnimationBlendVector.y;
        _characterController.Move(transform.TransformDirection(_moveDirection) * _speed * Time.fixedDeltaTime);
        _animator.SetFloat(_moveXAnimationParameterID, _currentAnimationBlendVector.x);
        _animator.SetFloat(_moveZAnimationParameterID, _currentAnimationBlendVector.y);

    }



    public void Jump()
    {
        if(_isGrounded && !_isCrouching)
        {
            if(_isSprinting)
            {
                _animator.CrossFade(_jumpAnimationSprint, _animationPlayTransition);
            }
            else if(!_isSprinting)
            {
                _animator.CrossFade(_jumpAnimationIdle, _animationPlayTransition);
            }
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -3.0f * _gravity);
        }
    }

    public void Crouch()
    {
        _isCrouching = !_isCrouching;
        _crouchTimer = 0;
        _lerpCrouch = true;
        if (_isCrouching)
        {
            _speed = _crouchSpeed;
        }
        else
        {
            _speed = _walkSpeed;
        }
    }

    private bool GroundCheck()
    {
        //isGrounded = characterController.isGrounded;
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _distanceToGround + 0.46f);
        return _isGrounded;
    }

    private void CrouchSprintCheck()
    {
        if(_isCrouching)
        {
            _isSprinting = false;
            _canSprint = false;
        }
        else if(!_isCrouching && !_isSprinted)
        {
            _canSprint = true;
        }
        else
        {
            _canSprint = false;
        }
    }

    private void CrouchCheck()
    {
        if (!_isCrouching)
        {
            _speed = _walkSpeed;
        }
    }

    private void CrouchFunctionality()
    {
        //slows down crouch speed to make it more realistic
        if (_lerpCrouch)
        {
            _crouchTimer += Time.deltaTime;
            float crouchLerpValue = _crouchTimer / 1;
            crouchLerpValue *= crouchLerpValue;
            if (_isCrouching)
            {
                _characterController.height = Mathf.Lerp(_characterController.height, 1, crouchLerpValue);
            }
            else
            {
                _characterController.height = Mathf.Lerp(_characterController.height, 2, crouchLerpValue);
            }
            if (crouchLerpValue > 1)
            {
                _lerpCrouch = false;
                _crouchTimer = 0f;
            }
        }
    }

    public void SprintStart()
    {
        
        if (!_isCrouching && _canSprint && !_isCrouching)
        {

            _isSprinting = true;
            _speed = _sprintSpeed;
            StartCoroutine(SprintDuration());
            _isSprinted = true;
            _canSprint = false;
            
        }

    }
    public void SprintFinish()
    {
        _isSprinting = false;
        if (!_isCrouching)
        {
            _speed = _walkSpeed;
        }
    }

    IEnumerator SprintDuration()
    {
        yield return new WaitForSeconds(3);
        CrouchCheck();
        _isSprinting = false;
        yield return new WaitForSeconds(3);
        _isSprinted = false;
        _canSprint = true;
    }
///<summary>
///end of movement codes.
/// </summary>


}
