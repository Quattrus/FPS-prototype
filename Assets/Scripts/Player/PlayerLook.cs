using UnityEngine;
using Cinemachine;

public class PlayerLook : MonoBehaviour
{
    [Header("Player look")]
    private float xRotation = 0f;
    [SerializeField] float xSensitivity = 30f;
    [SerializeField] float ySensitivity = 30f;
    [SerializeField] Transform playerCameraRoot;

    [Header("Aim Functionality")]
    [SerializeField] int aimCamPriority = 10;
    [SerializeField] CinemachineVirtualCamera aimCamera;
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
        xRotation = Mathf.Clamp(xRotation, -75f, 75f);
        //apply it to the camera rotation.
        playerCameraRoot.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //rotates the player to look left and right according to the face movement.
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
        aimCamera.Priority += aimCamPriority;
        xSensitivity = 10f;
        ySensitivity = 10f;
        isAiming = !isAiming;
    }
    public void PlayerAimFinished()
    {
        aimCamera.Priority -= aimCamPriority;
        xSensitivity = 30f;
        ySensitivity = 30f;
        isAiming = !isAiming;
    }
///<summary>
///end of player aiming codes
/// </summary>
}
