using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Initialization")]
    private CharacterController _characterController;
    private Vector3 _playerVelocity;
    //animations
    private Animator _animator; //done
    private int _moveXAnimationParameterID;
    private int _moveZAnimationParameterID;
    private int _jumpAnimationIdle; //done
    private int _jumpAnimationSprint; //done
    [SerializeField] float _animationPlayTransition = 0.15f; //done
    Vector3 _moveDirection = Vector3.zero;

    [Header("Animation Smoothing")]
    private Vector2 _currentAnimationBlendVector;
    Vector2 _animationVelocity;
    [SerializeField] float _animationSmoothTime = 0.1f;


    [Header("Player Movement Checks")]
    [SerializeField] private bool _isSprinting;//done
    [SerializeField] private bool _isIdle; //done
    [SerializeField] private bool _canSprint; //done
    [SerializeField] bool _isGrounded;//done
    [SerializeField] bool _jumped;//Done
    private bool _isCrouching = false;
    private bool _lerpCrouch = false;
    [SerializeField] float _distanceToGround; //done
    [SerializeField] bool _isFalling;//done


    [Header("Player Movement Variables")]
    [SerializeField] float _gravity = -9.8f;//done
    [SerializeField] float _speed = 5f; //done
    [SerializeField] float _jumpHeight = 3f; //done
    [SerializeField] float _crouchTimer = 5f;

    [Header("Movement Speed")]
    [SerializeField] float _sprintSpeed = 8f; //done
    [SerializeField] float _walkSpeed = 5f; //done
    [SerializeField] float _crouchSpeed = 2f;

    [Header("State Variables")]
    PlayerBaseState _currentState;//Done
    PlayerStateFactory _states;

    //Getters and Setters
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public bool Jumped { get { return _jumped; } set { _jumped = value; } }
    public bool IsSprinting { get { return _isSprinting; } set { _isSprinting = value; } }
    public Animator Animator { get { return _animator;} set { _animator = value; } }
    public int JumpAnimationSprint { get { return _jumpAnimationSprint; } set { _jumpAnimationSprint = value; } }
    public float AnimationPlayTransition { get { return _animationPlayTransition; } set { _animationPlayTransition = value; } }
    public int JumpAnimationIdle { get { return _jumpAnimationIdle; } set { _jumpAnimationIdle = value; } }
    public Vector3 PlayerVelocity { get { return _playerVelocity; } set { _playerVelocity = value; } }
    public float PlayerVelocityX { get { return _playerVelocity.x; } set { _playerVelocity.x = value; } }
    public float PlayerVelocityY { get{ return _playerVelocity.y; } set { _playerVelocity.y = value; } }
    public float PlayerVelocityZ { get { return _playerVelocity.z; }set { _playerVelocity.z = value; } }
    public float JumpHeight { get { return _jumpHeight; } set { _jumpHeight = value; } }
    public float Gravity { get { return _gravity; } set { _gravity = value; } }
    public bool IsGrounded { get { return _isGrounded; } set { _isGrounded = value; } }
    public bool IsFalling { get { return _isFalling; } set { _isFalling = value; } }
    public float DistanceToGround { get { return _distanceToGround; } set { _distanceToGround = value; } }
    public bool IsIdle { get { return _isIdle; } set { _isIdle = value; } }
    public float SprintSpeed { get { return _sprintSpeed; } set { _sprintSpeed = value; } }
    public float Speed { get { return _speed; } set { _speed = value; } }
    public float WalkSpeed { get { return _walkSpeed; } set { _walkSpeed = value; } }
    public bool CanSprint { get { return _canSprint; } set { _canSprint = value; } }
    void Awake()
    {
        ///<summary>
        ///These are all movement related.
        /// </summary>
        _characterController = GetComponent<CharacterController>();
        _distanceToGround = GetComponent<CharacterController>().bounds.extents.y;
        _canSprint = true;
        _speed = _walkSpeed;

        //state machine
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

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

    }

    private void Update()
    {
        _currentState.UpdateStates();
        GroundCheck();
        FallCheck();
    }

    public void Jump()
    {
        _jumped = true;
    }

    public void ProcessMove(Vector2 input)
    {
        if (_isSprinting)
        {
            input.x += Mathf.Lerp(input.x, 1, 1 * Time.deltaTime);
            input.y += Mathf.Lerp(input.y, 1, 1 * Time.deltaTime);
        }

        _playerVelocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);

        //smoothdamp
        _currentAnimationBlendVector = Vector2.SmoothDamp(_currentAnimationBlendVector, input, ref _animationVelocity, _animationSmoothTime);
        _moveDirection.x = _currentAnimationBlendVector.x;
        _moveDirection.z = _currentAnimationBlendVector.y;
        _characterController.Move(transform.TransformDirection(_moveDirection) * _speed * Time.deltaTime);
        _animator.SetFloat(_moveXAnimationParameterID, _currentAnimationBlendVector.x);
        _animator.SetFloat(_moveZAnimationParameterID, _currentAnimationBlendVector.y);

    }

    public void SprintStart()
    {
        if(_canSprint)
        {
           _isSprinting = true;
           _canSprint = false;
           StartCoroutine(SprintDuration());
        }

    }
    public void SprintFinish()
    {
        _isSprinting = false;
    }

    IEnumerator SprintDuration()
    {
        yield return new WaitForSeconds(3);
        _isSprinting = false;
        yield return new WaitForSeconds(3);
        _canSprint = true;
    }


    public void IdleCheck(Vector2 input)
    {
        if (input == Vector2.zero)
        {
            _isIdle = true;
        }
        else
        {
            _isIdle = false;
        }
    }

    private bool GroundCheck()
    {
        //isGrounded = characterController.isGrounded;
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _distanceToGround + 0.46f);
        return _isGrounded;
    }

    private bool FallCheck()
    {
        if(!_jumped)
        {
            _isFalling = !_isGrounded;
        }
        else
        {
            _isFalling = _playerVelocity.y <= 3f;
        }
          
        return _isFalling;
    }




}
