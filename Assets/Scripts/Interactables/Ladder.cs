using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class Ladder : Interactable
{
    [SerializeField] GameObject _player;

    protected override void Interact()
    {
        if (!_player.GetComponent<PlayerStateMachine>().IsClimbing)
        {
            _player.GetComponent<CharacterController>().enabled = false;
            _player.GetComponent<PlayerStateMachine>().Gravity = 0;
            _player.GetComponent<PlayerStateMachine>().TransitionClimb = true;
        }

    }
}
