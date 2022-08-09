using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    private PlayerUI playerUI;
    private InputManager inputManager;


    [SerializeField]private float distance = 3f;
    [SerializeField] LayerMask rayCastMask;



 
    void Start()
    {
        cam = GetComponent<PlayerLook>().Cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
    }


    void Update()
    {
        PlayerInteraction();

    }

    private void PlayerInteraction()
    {
        playerUI.UpdateText(string.Empty);
        //create a ray at the center of the camera, shooting outwards.
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo; //variable that stores our collision information.
        if (Physics.Raycast(ray, out hitInfo, distance, rayCastMask))
        {

            //checks if the object has an interactable component
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                //if it has then the UI will update the text to the prompt message.
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.promptMessage);
                if (inputManager.OnFoot.Interact.triggered)
                {
                    interactable.BaseInteract();
                }
            }
        }
    }
}
