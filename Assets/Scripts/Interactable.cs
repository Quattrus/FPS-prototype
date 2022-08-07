using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    //message displayed to the player when looking at an interactable.
    public string promptMessage;


    public void BaseInteract()
    {
        Interact();
    }
    protected virtual void Interact()
    {
        //there won't be any code written in this function
        //it is only a template function to be overridden by the subclasses.
    }
}
