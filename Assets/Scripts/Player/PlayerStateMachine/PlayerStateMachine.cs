using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    #region Variables
    [Header("Initialization")]
    private CharacterController _characterController;
    private Vector3 _playerVelocity;
    private IKBehaviour footIKBehaviour;
    private StaminaController _staminaController; //done

    //animations
    private Animator _animator; //done
    private int _moveXAnimationParameterID;
    private int _moveZAnimationParameterID;
    private int _moveXCrouchAnimationParameterID;//done
    private int _moveZCrouchAnimationParameterID;//done
    private int _defaultAirAnimation; //done
    private int _landAnimation; // done
    [SerializeField] float _animationPlayTransition = 0.15f; //done
    Vector3 _moveDirection = Vector3.zero;

    [Header("Animation Smoothing")]
    private Vector2 _currentAnimationBlendVector; //done
    Vector2 _animationVelocity;
    [SerializeField] float _animationSmoothTime = 0.1f; //done


    [Header("Player Movement Checks")]
    [SerializeField] private bool _isSprinting;//done
    [SerializeField] private bool _isIdle; //done
    [SerializeField] private bool _canSprint; //done
    [SerializeField] bool _isGrounded;//done
    [SerializeField] bool _jumped = false;//Done
    [SerializeField] bool _isInAir = false; //Done
    private bool _isCrouching = false;//done
    private bool _lerpCrouch = false;
    [SerializeField] bool _isFalling = false;//done
    [SerializeField] bool _enableFootIK = true;


    [Header("Player Movement Variables")]
    [SerializeField] float _gravity = -9.8f;//done
    [SerializeField] float _speed = 5f; //done
    [SerializeField] float _jumpHeight = 3f; //done
    [SerializeField] float _crouchTimer = 5f;

    [Header("Movement Speed")]
    [SerializeField] float _sprintSpeed = 8f; //done
    [SerializeField] float _walkSpeed = 5f; //done
    [SerializeField] float _crouchSpeed = 2f; //done

    [Header("State Variables")]
    PlayerBaseState _currentState;//Done
    PlayerStateFactory _states;

    [Header("GroundCheck")]
    [SerializeField] float sphereRadius;
    [SerializeField] float maxDistanceToGround;
    private float groundDistance;
    [SerializeField]  LayerMask sphereCastMask;
    private Vector3 sphereCastOrigin;
    [SerializeField] GameObject ground;

    [Header("Wall Check")]
    [SerializeField] float targetWallDistance;
    private Vector3 lowWallTargetOrigin;
    private Vector3 highWallTargetOrigin;
    [Range(0, 5)]
    [SerializeField] float heightOffset = 0.5f;
    private Vector3 lowWallDirection;
    private Vector3 highWallDirection;
    [SerializeField] LayerMask wallCastMaskLayer;
    [SerializeField] bool _canVault;
    [SerializeField] bool _isVaulting = false;
    [SerializeField] float wallCastRadius;
    [SerializeField] bool _gotLowWall = false;
    [SerializeField] bool _gotHighWall = false;


    #endregion
 

    #region Getters and Setters
    //Getters and Setters
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public bool Jumped { get { return _jumped; } set { _jumped = value; } }
    public bool IsSprinting { get { return _isSprinting; } set { _isSprinting = value; } }
    public Animator Animator { get { return _animator;} set { _animator = value; } }
    public int DefaultAirAnimation { get { return _defaultAirAnimation;} set { _defaultAirAnimation = value; } }
    public int LandAnimation { get { return _landAnimation; } set { _landAnimation = value; } }
    public float AnimationPlayTransition { get { return _animationPlayTransition; } set { _animationPlayTransition = value; } }
    public Vector3 PlayerVelocity { get { return _playerVelocity; } set { _playerVelocity = value; } }
    public float PlayerVelocityX { get { return _playerVelocity.x; } set { _playerVelocity.x = value; } }
    public float PlayerVelocityY { get{ return _playerVelocity.y; } set { _playerVelocity.y = value; } }
    public float PlayerVelocityZ { get { return _playerVelocity.z; }set { _playerVelocity.z = value; } }
    public float JumpHeight { get { return _jumpHeight; } set { _jumpHeight = value; } }
    public float Gravity { get { return _gravity; } set { _gravity = value; } }
    public bool IsGrounded { get { return _isGrounded; } set { _isGrounded = value; } }
    public bool IsFalling { get { return _isFalling; } set { _isFalling = value; } }
    public bool IsIdle { get { return _isIdle; } set { _isIdle = value; } }
    public float SprintSpeed { get { return _sprintSpeed; } set { _sprintSpeed = value; } }
    public float Speed { get { return _speed; } set { _speed = value; } }
    public float WalkSpeed { get { return _walkSpeed; } set { _walkSpeed = value; } }
    public bool CanSprint { get { return _canSprint; } set { _canSprint = value; } }
    public bool IsInAir { get { return _isInAir; } set { _isInAir = value; } }
    public float CrouchSpeed { get { return _crouchSpeed; } set { _crouchSpeed = value; } }
    public bool IsCrouching { get { return _isCrouching; } set { _isCrouching = value; } }
    public int MoveXCrouchAnimationParameterID { get { return _moveXCrouchAnimationParameterID; } set { _moveXCrouchAnimationParameterID = value; } }
    public int MoveZCrouchAnimationParameterID { get { return _moveZCrouchAnimationParameterID; } set { _moveZCrouchAnimationParameterID = value; } }
    public Vector2 CurrentAnimationBlendVector { get { return _currentAnimationBlendVector; } set { _currentAnimationBlendVector = value; } }
    public float AnimationSmoothTime { get { return _animationSmoothTime; } set { _animationSmoothTime = value; } }
    public bool EnableFootIK { get { return _enableFootIK; } set { _enableFootIK = value; } }
    public StaminaController StaminaController { get { return _staminaController; } set { _staminaController = value; } }
    public bool GotHighWall { get { return _gotHighWall; } set { _gotHighWall = value; } }
    public bool GotLowWall { get { return _gotLowWall; } set { _gotLowWall = value; } }
    #endregion
    void Awake()
    {
        ///<summary>
        ///These are all movement related.
        /// </summary>
        _characterController = GetComponent<CharacterController>();
        _canSprint = true;
        _speed = _walkSpeed;
        footIKBehaviour = GetComponent<IKBehaviour>();
        _staminaController = GetComponent<StaminaController>();
        

        //state machine
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        //animations
        _animator = GetComponent<Animator>();
        _moveXAnimationParameterID = Animator.StringToHash("MoveX");
        _moveZAnimationParameterID = Animator.StringToHash("MoveY");
        _moveXCrouchAnimationParameterID = Animator.StringToHash("MoveXCrouch");
        _moveZCrouchAnimationParameterID = Animator.StringToHash("MoveZCrouch");
        //animator.SetFloat(moveXAnimationParameterID, 1f);

    }

    private void FixedUpdate()
    {
        
    }

    private void Update()
    {
        TerminalVelocity();
        _currentState.UpdateStates();
        GroundCheck();
        FallCheck();
        CrouchFunctionality();
        FootIKCheck();
        LowWallCheck();
        HighWallCheck();

    }


    #region Player Movement
    public void Jump()
    {
        _jumped = true;
        footIKBehaviour.EnableFeetIK = false;
    }

    public void ProcessMove(Vector2 input)
    {
        if (_isSprinting)
        {
            input.y += Mathf.Lerp(input.y, 1, 1 * Time.deltaTime);
        }

        if (!_isFalling && _playerVelocity.y < -2f)
        {
            _playerVelocity.y = -2f;
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
                _characterController.height = Mathf.Lerp(_characterController.height, 1.31f, crouchLerpValue);
                _animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 1f, crouchLerpValue));
            }
            else
            {
                _characterController.height = Mathf.Lerp(_characterController.height, 1.7f, crouchLerpValue);
                _animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 0f, crouchLerpValue));
            }
            if (crouchLerpValue > 1)
            {
                _lerpCrouch = false;
                _crouchTimer = 0f;
            }
        }
    }
    #endregion

    #region PlayerMovementChecks
    public void SprintStart()
    {
        if(_canSprint)
        {
           _isSprinting = true;
           _canSprint = false;
        }

    }

    public void Crouch()
    {
        _isCrouching = !_isCrouching;
        _crouchTimer = 0;
        _lerpCrouch = true;
    }
    public void SprintFinish()
    {
        _isSprinting = false;
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
        sphereCastOrigin = transform.position;
        RaycastHit hit;
        if (Physics.SphereCast(sphereCastOrigin, sphereRadius, Vector3.down, out hit, maxDistanceToGround, sphereCastMask, QueryTriggerInteraction.UseGlobal))
        {
            ground = hit.transform.gameObject;
            groundDistance = hit.distance;
            
        }
        else
        {
            groundDistance = maxDistanceToGround;
            ground = null;

        }
        if(ground != null)
        {
          _isGrounded = true;
        }
        else if(ground == null)
        {
            _isGrounded = false;
        }
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
            _isFalling = _playerVelocity.y < -2f;
        }
          
        return _isFalling;
    }

    private void TerminalVelocity()
    {
        if(_playerVelocity.y <= -200f)
        {
            _playerVelocity.y = -200f;
        }
    }

    private void FootIKCheck()
    {
        footIKBehaviour.EnableFeetIK = _enableFootIK;
    }

    private void LowWallCheck()
    {
        RaycastHit lowWallHit = new RaycastHit();
        //int numberOfHits = 0;
        //for(int i = -1; i < 2; i++)
        //{
        lowWallTargetOrigin = transform.position;
        //wallTargetOrigin += transform.right * (i*0.35f);

        lowWallDirection = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(lowWallTargetOrigin, lowWallDirection * targetWallDistance, Color.magenta);
        if (Physics.SphereCast(lowWallTargetOrigin, wallCastRadius, lowWallDirection, out lowWallHit, targetWallDistance, wallCastMaskLayer))
        {
            _gotLowWall = true;
            //numberOfHits++;
        }
        else
        {
            _gotLowWall = false;
        }

        //}
        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.magenta);

        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out wallHit))
        //{

        //    targetWallDistance = wallHit.distance;
        //}
    }
    private void HighWallCheck()
    {
        RaycastHit highWallHit = new RaycastHit();
        highWallTargetOrigin = new Vector3(transform.position.x, transform.position.y + heightOffset, transform.position.z);
        highWallDirection = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(highWallTargetOrigin, highWallDirection * targetWallDistance, Color.blue);
        if(Physics.SphereCast(highWallTargetOrigin, wallCastRadius, highWallDirection, out highWallHit, targetWallDistance, wallCastMaskLayer))
        {
            _gotHighWall = true;
        }
        else
        {
            _gotHighWall = false;
        }
    }


    #endregion

    #region Debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(sphereCastOrigin, sphereCastOrigin + Vector3.down * groundDistance);
        Gizmos.DrawWireSphere(sphereCastOrigin + Vector3.down * groundDistance, sphereRadius);
    }
    #endregion
}
