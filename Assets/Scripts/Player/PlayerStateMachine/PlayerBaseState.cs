using Player;
public abstract class PlayerBaseState
{
    private bool _isRootState = false;
    private PlayerStateMachine _ctx;
    private PlayerStateFactory _factory;
    private PlayerBaseState _currentSubState;
    private PlayerBaseState _currentSuperState;

    //Getters and Setters
    protected bool IsRootState {  get { return _isRootState; } set { _isRootState = value; } }
    protected PlayerStateMachine Ctx { get { return _ctx; } set { _ctx = value; } }
    protected PlayerStateFactory Factory { get { return _factory; } set{_factory = value;} }
    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubstate();
    public void UpdateStates()
    {
        UpdateState();
        if(_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }
   protected void SwitchState(PlayerBaseState newState)
    {
        //current state exit state
        ExitState();
        //new state enters state
        newState.EnterState();
        if(_isRootState)
        {
           //switch current state of context
           _ctx.CurrentState = newState;
        }
        else if(_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }

    }
    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(PlayerBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
