using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Interactable
{
    [SerializeField] GameObject _player;

    protected override void Interact()
    {
        _player.GetComponent<CharacterController>().enabled = false;
        _player.GetComponent<PlayerStateMachine>().IsClimbing = true;
    }
}
