using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerStateMachine : MonoBehaviour
{
    public static PlayerStateMachine Instance { get; private set; }
    #region Variables

    #region Initialization Variables
    private CharacterController _characterController; //done
    private CapsuleCollider _collider;
    private Vector3 _playerVelocity;
    private IKBehaviour _footIKBehaviour;
    private Inventory _inventory;
    private StaminaController _staminaController; //done
    [SerializeField] Transform _playerCameraRoot;
    [SerializeField] Transform _aimTarget;
    [SerializeField] Transform _bodyHeadAimTarget;
    [SerializeField] CinemachineVirtualCamera _aimCamera;
    #endregion


    #region Player Look
    private float _xRotation = 0f;
    [SerializeField] float _xSensitivity = 30f;
    [SerializeField] float _ySensitivity = 30f;
    private float _xSensitivityAim;
    private float _xSensitivityDefault;
    private float _ySensitivityAim;
    private float _ySensitivityDefault;
    #endregion


    [SerializeField] LayerMask _wallCheckLayerMask;
    private float _lowWallDistance;
    private float _highWallDistance;


    #region Animation Variables
    private Animator _animator; //done
    private int _moveXAnimationParameterID, _moveZAnimationParameterID, _moveXCrouchAnimationParameterID, _moveZCrouchAnimationParameterID, _defaultAirAnimation, _landAnimation;
    [SerializeField] float _animationPlayTransition = 0.15f; //done
    #endregion


    Vector3 _moveDirection = Vector3.zero;
    [SerializeField] bool _meleeMode = false;
    [SerializeField] GameObject _meleeWheel;



    [Header("Aim Functionality")]
    [SerializeField] int _aimCamPriority = 10;
    [SerializeField] bool _isAiming;
    [SerializeField] GameObject _chestBone;

    [Header("Weapons")]
    [SerializeField] bool _isArmed;

    [Header("Animation Smoothing")]
    private Vector2 _currentAnimationBlendVector, _animationVelocity;
    [SerializeField] float _animationSmoothTime = 0.1f; //done


    [Header("Player Movement Checks")]
    [SerializeField] private bool _isSprinting, _isIdle, _canSprint, _isGrounded, _jumped, _isInAir, _isCrouching, _lerpCrouch, _isFalling = false;
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
    [SerializeField] float _stairClimbSpeed = 0.75f;

    [Header("State Variables")]
    PlayerBaseState _currentState;//Done
    PlayerStateFactory _states;

    [Header("GroundCheck")]
    [SerializeField] float _groundCastRadius;
    [SerializeField] float _maxDistanceToGround;
    private float _groundDistance;
    private float _initialGroundDistance;
    [SerializeField] LayerMask _groundCastMask;
    private Vector3 _groundCastOrigin;
    [SerializeField] GameObject _ground;

    [Header("Wall Check")]
   // [SerializeField] float _targetWallDistance;
    private Vector3 _lowWallTargetOrigin, _highWallTargetOrigin, _groundFrontTargetOrigin, _kneeWallTargetOrigin, _kneeWallBackTargetOrigin;
    [Range(0, 1)]
    [SerializeField] float _slopeLayerWeightValue = 0.5f;
    [Range(-5, 5)]
    [SerializeField] float _kneeLevelOffset;
    [Range(-5, 5)]
    [SerializeField] float _heightOffset = 0.5f;
    [Range(-5, 5)]
    [SerializeField] float _kneeBackLevelOffset;
    [SerializeField] LayerMask _wallCastMaskLayer;
    [SerializeField] bool _canVault;
    [SerializeField] bool _isVaulting = false; //done
    [SerializeField] float wallCastRadius;
    [SerializeField] bool _gotLowWall = false;
    [SerializeField] bool _gotHighWall = false;
    [SerializeField] bool _gotNeFoot = false;
    [SerializeField] bool _gotNeFootBack = false;
    [SerializeField] GameObject _groundFrontCheck;
    private float _groundFrontDistance, _kneeWallDistance, _kneeWallBackDistance;
    private Vector3 _targetVaultPosition;
    private bool _startVault;
    [Range(0, 10)]
    [SerializeField] float _groundFrontCheckDistance, _highWallCheckDistance = 5f;
    [Range(0, 10)]
    [SerializeField] float _kneeWallBackCheckDistance, _kneeWallCheckDistance = 2f;
    [Range(0, 10)]
    [SerializeField] float _lowWallCheckDistance = 3f;

    [Header("ClimbCheck")]
    [SerializeField] bool _isClimbing;
    [SerializeField] bool _canClimb;
    private Vector3 _highWallHitPoint = Vector3.zero;
    private Transform _ladderBoundsTransformPosition;
    [SerializeField] bool _climbTransition;
    [SerializeField] bool _climbExit;
    private Transform _ladderExitPosition;


    #endregion


    #region Getters and Setters
    //Getters and Setters
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public bool Jumped { get { return _jumped; } set { _jumped = value; } }
    public bool IsSprinting { get { return _isSprinting; } set { _isSprinting = value; } }
    public Animator Animator { get { return _animator; } set { _animator = value; } }
    public int DefaultAirAnimation { get { return _defaultAirAnimation; } set { _defaultAirAnimation = value; } }
    public int LandAnimation { get { return _landAnimation; } set { _landAnimation = value; } }
    public float AnimationPlayTransition { get { return _animationPlayTransition; } set { _animationPlayTransition = value; } }
    public Vector3 PlayerVelocity { get { return _playerVelocity; } set { _playerVelocity = value; } }
    public float PlayerVelocityX { get { return _playerVelocity.x; } set { _playerVelocity.x = value; } }
    public float PlayerVelocityY { get { return _playerVelocity.y; } set { _playerVelocity.y = value; } }
    public float PlayerVelocityZ { get { return _playerVelocity.z; } set { _playerVelocity.z = value; } }
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
    public bool CanVault { get { return _canVault; } set { _canVault = value; } }
    public Vector3 TargetVaultPosition { get { return _targetVaultPosition; } set { _targetVaultPosition = value; } }
    public bool IsVaulting { get { return _isVaulting; } set { _isVaulting = value; } }
    public CharacterController CharacterController { get { return _characterController; } set { _characterController = value; } }
    public bool StartVault { get { return _startVault; } set { _startVault = value; } }
    public CapsuleCollider Collider { get { return _collider; } set { _collider = value; } }
    public bool IsAiming {get{return _isAiming;} set{_isAiming = value;}}
    public bool IsClimbing { get { return _isClimbing; } set { _isClimbing = value; } }
    public Transform LadderBoundsTransformPosition { get { return _ladderBoundsTransformPosition; }set { _ladderBoundsTransformPosition = value; } }
    public bool ClimbTransition { get { return _climbTransition; } set { _climbTransition = value; } }
    public bool ClimbExit { get { return _climbExit; }set { _climbExit = value; } }
    public bool IsArmed { get { return _isArmed; } set { _isArmed = value; } }
    public bool MeleeMode { get { return _meleeMode; } set { _meleeMode = value; } }
    #endregion
    void Awake()
    {
        Instance = this;
        ///<summary>
        ///These are all movement related.
        /// </summary>
        _inventory = GetComponent<Inventory>();
        _characterController = GetComponent<CharacterController>();
        _collider = GetComponent<CapsuleCollider>();
        _collider.enabled = false;
        _canSprint = true;
        _speed = _walkSpeed;
        _footIKBehaviour = GetComponent<IKBehaviour>();
        _staminaController = GetComponent<StaminaController>();
        _meleeWheel.gameObject.SetActive(false);
        _initialGroundDistance = _groundDistance;
        

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


        //aiming
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _xSensitivityAim = _xSensitivity / 2;
        _ySensitivityAim = _ySensitivity / 2;
        _xSensitivityDefault = _xSensitivity;
        _ySensitivityDefault = _ySensitivity;

    }

    private void FixedUpdate()
    {
        if(!_isCrouching)
        {
            SlopeCheck();
        }
        CanVaultCheck();
        FootIKCheck();
        LowWallCheck();
        HighWallCheck();
        KneeWallCheck();
        HeadAndBodyAnim();
        KneeWallBackCheck();
        GroundCheck();
        GroundFrontCheck();
        Grounding();
    }

    private void Update()
    {
        if (_inventory.GunEquipped)
        {
            _isArmed = true;
        }
        if (_climbTransition)
        {
            StartCoroutine(TransitionClimbMove());
        }
        TerminalVelocity();
        _currentState.UpdateStates();
        FallCheck();
        CrouchFunctionality();

        if(_meleeMode)
        {
            _meleeWheel.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if(!_meleeMode)
        {
            _meleeWheel.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    #region Player Movement
    public void Jump()
    { 

        if (_canVault && !_isIdle)
        {
            _startVault = true;
            Vault();
        }
        else if(_canVault && _isIdle)
        {
            _jumped = true;
        }
        else
        {
            _jumped = true;
            //footIKBehaviour.EnableFeetIK = false;
        }

    }

   public void Vault()
    {
        StartCoroutine(VaultMove());
        _isVaulting = true;
        _canVault = false;
    }

    public IEnumerator TransitionClimbMove()
    {
        _playerVelocity = Vector3.zero;
        _collider.enabled = true;
        yield return new WaitForSeconds(1);
        _climbTransition = false;
        _collider.enabled = false;
        _characterController.enabled = true;
    }
    public IEnumerator VaultMove()
    {
        _playerVelocity = Vector3.zero;
        yield return new WaitForSeconds(1);
        _isVaulting = false;
        _characterController.enabled = true;
        _collider.enabled = false;
    }

    public void ProcessMove(Vector2 input)
    {
            if (_isSprinting)
            {
                input.y += Mathf.Lerp(input.y, 1, 1 * Time.deltaTime);
                input.x += Mathf.Lerp(input.x, 1, 1 * Time.deltaTime);
            }

            if (!_isFalling && _playerVelocity.y < -2f)
            {
                _playerVelocity.y = -2f;
            }
            //smoothdamp
            _currentAnimationBlendVector = Vector2.SmoothDamp(_currentAnimationBlendVector, input, ref _animationVelocity, _animationSmoothTime);
            _moveDirection.x = _currentAnimationBlendVector.x;
            _moveDirection.z = _currentAnimationBlendVector.y;
            _characterController.Move(transform.TransformDirection(_moveDirection) * _speed * Time.deltaTime);
            _animator.SetFloat(_moveXAnimationParameterID, _currentAnimationBlendVector.x);
            _animator.SetFloat(_moveZAnimationParameterID, _currentAnimationBlendVector.y);
    }

    public void ProcessClimb(Vector2 input)
    {
        //_moveDirection.x = input.x;
        //_moveDirection.y = input.y;
        _playerVelocity.y = input.y;
        _characterController.Move(_playerVelocity * Time.deltaTime);
        //_characterController.Move(transform.TransformDirection(_moveDirection) * _speed * Time.deltaTime);
    }

    public void ProcessLook(Vector2 input)
    {   
        float mouseX = input.x;
        float mouseY = input.y;
        //calculates the camera rotation for looking up and down
        _xRotation -= (mouseY * Time.deltaTime) * _ySensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -60f, 60f);
        //apply rotation to the camera.
        _playerCameraRoot.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        _chestBone.transform.localRotation = _playerCameraRoot.transform.localRotation;
        //rotate the player to look left and right according to the camera.
        Vector3 rotationValue = (Vector3.up * (mouseX * Time.deltaTime) * _xSensitivity);
        transform.Rotate(rotationValue);

        //Find a way to add the turn animation here.
        // if(_isIdle && rotationValue.y <= -1f)
        // {
        //     Debug.Log("rotate");
        //    _animator.SetLayerWeight(4, Mathf.Lerp(_animator.GetLayerWeight(1), 1f, 1f));
        //    _animator.SetTrigger("TurnLeft");
        // }
        // else if(_isIdle && rotationValue.y >= 1f)
        // {
        //     Debug.Log("rotate right");
        //    _animator.SetLayerWeight(4, Mathf.Lerp(_animator.GetLayerWeight(1), 1f, 1f));
        //    _animator.SetTrigger("TurnRight");
        //}
        // else
        //{
        //    _animator.SetLayerWeight(4, Mathf.Lerp(_animator.GetLayerWeight(1), 0f, 1f));
        //}
    }

    private void HeadAndBodyAnim()
    {
        _aimTarget.position = _bodyHeadAimTarget.position;
    }


    public void PlayerAimStart()
    {
        _aimCamera.Priority += _aimCamPriority;
        _xSensitivity = _xSensitivityAim;
        _ySensitivity = _ySensitivityAim;
        _isAiming = !_isAiming;
    }
    public void PlayerAimFinished()
    {
        _aimCamera.Priority -= _aimCamPriority;
        _xSensitivity = _xSensitivityDefault;
        _ySensitivity = _ySensitivityDefault;
        _isAiming = !_isAiming;
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
                _footIKBehaviour.PelvisOffset = 0.75f;
                _animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 1f, crouchLerpValue));
            }
            else
            {
                _characterController.height = Mathf.Lerp(_characterController.height, 1.7f, crouchLerpValue);
                _footIKBehaviour.PelvisOffset = 0.83f;
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

    private void Grounding()
    {
        if(!_isClimbing)
        {
            _playerVelocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_playerVelocity * Time.deltaTime);
        }

    }

    private bool GroundCheck()
    {
        _groundCastOrigin = transform.position;
        RaycastHit hit;
        if (Physics.SphereCast(_groundCastOrigin, _groundCastRadius, Vector3.down, out hit, _maxDistanceToGround, _groundCastMask, QueryTriggerInteraction.UseGlobal))
        {
            _groundDistance = Vector3.Distance(hit.point, _groundCastOrigin);
            _ground = hit.transform.gameObject;
            _groundDistance = hit.distance;
            
        }
        else
        {
            _groundDistance = _maxDistanceToGround;
            _ground = null;

        }
        if(_ground != null)
        {
          _isGrounded = true;
        }
        else if(_ground == null)
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
            _isFalling = _playerVelocity.y <= -2f;
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
        _footIKBehaviour.EnableFeetIK = _enableFootIK;
    }

    private void LowWallCheck()
    {
        RaycastHit lowWallHit = new RaycastHit();
        _lowWallTargetOrigin = transform.position;
        
        
        if (Physics.Raycast(_lowWallTargetOrigin, transform.forward, out lowWallHit, _lowWallCheckDistance, _wallCheckLayerMask))
        {
            _lowWallDistance = Vector3.Distance(lowWallHit.point, _lowWallTargetOrigin);
            _gotLowWall = true;
        }
        else
        {
            _gotLowWall = false;
        }
        Debug.DrawRay(_lowWallTargetOrigin, transform.forward, Color.magenta);
    }
    private void KneeWallCheck()
    {
        RaycastHit kneeWallHit = new RaycastHit();
        _kneeWallTargetOrigin = new Vector3(transform.position.x, transform.position.y + _kneeLevelOffset, transform.position.z);
        Debug.DrawRay(_kneeWallTargetOrigin, transform.forward, Color.black);
        if(Physics.Raycast(_kneeWallTargetOrigin, transform.forward,out kneeWallHit, _kneeWallCheckDistance, _wallCheckLayerMask))
        {
            _kneeWallDistance = Vector3.Distance(kneeWallHit.point, _kneeWallTargetOrigin);
            _gotNeFoot = true;
        }
        else
        {
            _gotNeFoot = false;
        }
    }

    private void KneeWallBackCheck()
    {
        RaycastHit kneeWallBackHit = new RaycastHit();
        _kneeWallBackTargetOrigin = new Vector3(transform.position.x, transform.position.y + _kneeBackLevelOffset, transform.position.z);
        Debug.DrawRay(_kneeWallBackTargetOrigin, -transform.forward, Color.black);
        if(Physics.Raycast(_kneeWallBackTargetOrigin, -transform.forward, out kneeWallBackHit, _kneeWallBackCheckDistance, _wallCheckLayerMask))
        {
            _kneeWallBackDistance = Vector3.Distance(kneeWallBackHit.point, _kneeWallBackTargetOrigin);
            _gotNeFootBack = true;
        }
        else
        {
            _gotNeFootBack = false;
        }
    }
    private void HighWallCheck()
    {
        RaycastHit highWallHit = new RaycastHit();
        _highWallTargetOrigin = new Vector3(transform.position.x, transform.position.y + _heightOffset, transform.position.z);
        Debug.DrawRay(_highWallTargetOrigin, transform.forward, Color.blue);
        if(Physics.Raycast(_highWallTargetOrigin, transform.forward, out highWallHit, _highWallCheckDistance, _wallCheckLayerMask))
        {
            _highWallHitPoint = highWallHit.point;
            _highWallDistance = Vector3.Distance(highWallHit.point, _highWallTargetOrigin);
            _gotHighWall = true;
        }
        else
        {
            _gotHighWall = false;
        }
    }

    private void GroundFrontCheck()
    {

            RaycastHit groundFront = new RaycastHit();
            _groundFrontTargetOrigin = _groundFrontCheck.transform.position;
            Debug.DrawRay(_groundFrontTargetOrigin, -transform.up, Color.green);
            if (Physics.Raycast(_groundFrontTargetOrigin, -transform.up, out groundFront, _groundFrontCheckDistance, _groundCastMask))
            {

                _groundFrontDistance = Vector3.Distance(groundFront.point, _groundFrontTargetOrigin);
                _targetVaultPosition = _groundFrontTargetOrigin;
                _targetVaultPosition.z = groundFront.point.z;
                _targetVaultPosition.y = groundFront.point.y + 1f;
            }
    }

    private void CanVaultCheck()
    {
        if(_lowWallDistance < 0.4 && _gotLowWall && !_gotHighWall || _lowWallDistance < 0.4 && _gotLowWall && _highWallDistance > 1)
        {
             _canVault = true;
        }
        else
        {
             _canVault = false;
        }
    }

    public void GetLadderBoundsPosition(Transform ladderBoundsPosition)
    {
        _ladderBoundsTransformPosition = ladderBoundsPosition;
    }

    public void GetLadderExitPosition(Transform ladderExitPosition)
    {
        _ladderExitPosition = ladderExitPosition;
    }

    private void SlopeCheck()
    {

        if(_gotNeFoot && _groundFrontDistance < _initialGroundDistance)
        {
            _animator.SetLayerWeight(2, Mathf.Lerp(_animator.GetLayerWeight(1), _slopeLayerWeightValue, _animationSmoothTime));
            
        }
        else if(!_gotNeFoot || _gotNeFoot && _lowWallDistance == _kneeWallDistance)
        {
            _animator.SetLayerWeight(2, Mathf.Lerp(_animator.GetLayerWeight(1), 0f, _animationSmoothTime));
        }
        if(_gotNeFootBack && _groundFrontDistance > _initialGroundDistance)
        {
            _animator.SetLayerWeight(3, Mathf.Lerp(_animator.GetLayerWeight(1), _slopeLayerWeightValue, _animationSmoothTime));
        }
        else if(!_gotNeFootBack || _groundFrontDistance == _initialGroundDistance)
        {
            _animator.SetLayerWeight(3, Mathf.Lerp(_animator.GetLayerWeight(1), 0f, _animationSmoothTime));
        }
    }





    #endregion

    #region Debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(_groundCastOrigin, _groundCastOrigin + Vector3.down * _groundDistance);
        Gizmos.DrawWireSphere(_groundCastOrigin + Vector3.down * _groundDistance, _groundCastRadius);
    }
    #endregion

    public void StrikeAnimations(int strikeType)
    {
        Debug.Log(strikeType);
        //Implement attack animations here.
            
    }
}
