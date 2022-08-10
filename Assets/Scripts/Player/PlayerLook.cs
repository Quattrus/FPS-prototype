using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Player look")]
    private float xRotation = 0f;
    [SerializeField] float xSensitivity = 30f;
    [SerializeField] float ySensitivity = 30f;
    [SerializeField] Camera cam;   
    public Camera Cam
    {
        get
        {
            return cam;
        }
    }


    [Header("Aim Functionality")]
    [SerializeField] float fovSpeed = 1f;
    [SerializeField] float aimFOV = 60f;
    private float initialFOV;
    [SerializeField] bool isAiming;
    public bool IsAiming
    {
        get { return isAiming; }
    }

    private void Start()
    {
///<summary>
///Player Look related
/// </summary>
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

///<summary>
///Player aim related
/// </summary>
        initialFOV = cam.fieldOfView;
    }
/// <summary>
/// basic look related
/// </summary>
    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        //calculate the camera rotation for looking up and down
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        //apply it to the camera rotatoion.
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //rotates the player to look left and right
        transform.Rotate(Vector3.up * (mouseX * Time.fixedDeltaTime) * xSensitivity);
    }
/// <summary>
/// End of basic look codes
/// </summary>

 ///<summary>
 ///player aiming related
 /// </summary>
    public void PlayerAimStart()
    {
        cam.fieldOfView = aimFOV += fovSpeed * Time.fixedDeltaTime;
        xSensitivity = 10f;
        ySensitivity = 10f;
        isAiming = !isAiming;
    }
    public void PlayerAimFinished()
    {
        cam.fieldOfView = initialFOV += fovSpeed * Time.fixedDeltaTime;
        xSensitivity = 30f;
        ySensitivity = 30f;
        isAiming = !isAiming;
    }
///<summary>
///end of player aiming codes
/// </summary>
}
