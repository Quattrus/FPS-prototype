using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Interactable
{
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _ladderBounds;
    [SerializeField] Transform _ladderBoundsPosition;
    [SerializeField] string _promptMessage1;
    private string originalPrompt;

    private bool isInteracted = false;


    private void Awake()
    {
        _ladderBounds.gameObject.SetActive(false);
        _ladderBoundsPosition = _ladderBounds.gameObject.GetComponent<Transform>();
        originalPrompt = promptMessage;
    }

    protected override void Interact()
    {

        if(!isInteracted)
        {
            _player.GetComponent<CharacterController>().enabled = false;
            _player.GetComponent<PlayerStateMachine>().IsClimbing = true;
            isInteracted = true;
            _ladderBounds.gameObject.SetActive(true);
            _player.GetComponent<PlayerStateMachine>().GetLadderBoundsPosition(_ladderBoundsPosition);
            promptMessage = _promptMessage1;
        }
        else if(isInteracted)
        {
            _player.GetComponent<PlayerStateMachine>().IsClimbing = false;
            isInteracted = false;
            _ladderBounds.gameObject.SetActive(false);
            promptMessage = originalPrompt;
        }

    }

    public void LadderBoundsCheck()
    {
        _player.GetComponent<PlayerStateMachine>().IsClimbing = false;
        isInteracted = false;
        _ladderBounds.gameObject.SetActive(false);
        promptMessage = originalPrompt;
    }
}
