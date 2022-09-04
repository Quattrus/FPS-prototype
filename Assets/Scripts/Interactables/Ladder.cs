using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class Ladder : Interactable
{
    [SerializeField] GameObject _player;
    [SerializeField] Transform _ladderExitBottom;
    [SerializeField] Transform _ladderExitTop;
    [SerializeField] Transform _ladderClimbStartBottom;
    [SerializeField] Transform _ladderClimbStartTop;
    public bool _canExit;

    public Transform LadderClimbStartBottom { get { return _ladderClimbStartBottom; } set { _ladderClimbStartBottom = value; } }
    public Transform LadderClimbStartTop { get { return _ladderClimbStartTop; } set { _ladderClimbStartTop = value; } }
    public Transform LadderExitTop { get { return _ladderExitTop; } set { _ladderExitTop = value; } }
    public Transform LadderExitBottom { get { return (_ladderExitBottom); } set { _ladderExitBottom = value; } }

    protected override void Interact()
    {
        if (!_player.GetComponent<PlayerStateMachine>().IsClimbing)
        {
            _player.GetComponent<CharacterController>().enabled = false;
            _player.GetComponent<PlayerStateMachine>().Gravity = 0;
            _player.GetComponent<PlayerStateMachine>().TransitionClimb = true;
            _player.GetComponent<PlayerStateMachine>().LadderClimbPosition(_ladderClimbStartBottom);
        }
        else if(_canExit && _player.GetComponent<PlayerStateMachine>().IsClimbing)
        {
            _player.GetComponent<PlayerStateMachine>().LadderClimbPosition(_ladderExitBottom);
            _player.GetComponent<PlayerStateMachine>().TransitionClimbExitCheck = true;
        }
    }


}
