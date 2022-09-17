using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Interactable
{
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _ladderBounds;
    [SerializeField] Transform _ladderBoundsPosition;
    [SerializeField] Transform _ladderExitPosition;
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
            PlayerStateMachine.Instance.enabled = false;
            PlayerStateMachine.Instance.ClimbTransition = true;
            PlayerStateMachine.Instance.IsClimbing = true;
            isInteracted = true;
            _ladderBounds.gameObject.SetActive(true);
            PlayerStateMachine.Instance.GetLadderBoundsPosition(_ladderBoundsPosition);
            promptMessage = _promptMessage1;
        }
        else if(isInteracted)
        {
            PlayerStateMachine.Instance.IsClimbing = false;
            isInteracted = false;
            _ladderBounds.gameObject.SetActive(false);
            promptMessage = originalPrompt;
        }

    }

    public void LadderBoundsCheck()
    {
        PlayerStateMachine.Instance.ClimbExit = true;
        PlayerStateMachine.Instance.IsClimbing = false;
        PlayerStateMachine.Instance.GetLadderBoundsPosition(_ladderExitPosition);
        isInteracted = false;
        _ladderBounds.gameObject.SetActive(false);
        promptMessage = originalPrompt;
    }
}
