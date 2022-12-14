
public class PlayerStateFactory 
{
    PlayerStateMachine _context;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
    }
    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(_context, this);
    }
    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(_context, this);
    }
    public PlayerBaseState Run()
    {
        return new PlayerRunState(_context, this);
    }
    public PlayerBaseState Jump()
    {
        return new PlayerJumpState(_context, this);
    }
    public PlayerBaseState Grounded()
    {
        return new PlayerGroundedState(_context, this);
    }
    public PlayerBaseState Falling()
    {
        return new PlayerFallState(_context, this);
    }
    public PlayerBaseState Crouching()
    {
        return new PlayerCrouchState(_context, this);
    }
    public PlayerBaseState Vaulting()
    {
        return new PlayerVaultState(_context, this);
    }
    public PlayerBaseState Climbing()
    {
        return new PlayerClimbState(_context, this);
    }
    public PlayerBaseState Armed()
    {
        return new PlayerArmedState(_context, this);
    }
}
