using UnityEngine;
using Cinemachine;

public class PlayerLook : MonoBehaviour
{
    [Header("Player look")]
    private float xRotation = 0f;
    [SerializeField] float xSensitivity = 30f;
    [SerializeField] float ySensitivity = 30f;
    private float xSensitivityAim;
    private float ySensitivityAim;
    private float xSensitivityDefault;
    private float ySensitivityDefault;
    [SerializeField] Transform playerCameraRoot;
    private Animator animator;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private Transform bodyHeadAimTarget;

    [Header("Aim Functionality")]
    [SerializeField] int aimCamPriority = 10;
    [SerializeField] CinemachineVirtualCamera aimCamera;
    [SerializeField] bool isAiming;
    public bool IsAiming
    {
        get { return isAiming; }
    }

    private void Awake()
    {
///<summary>
///Player Look related
/// </summary>
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        xSensitivityAim = xSensitivity / 2;
        ySensitivityAim = ySensitivity / 2;
        xSensitivityDefault = xSensitivity;
        ySensitivityDefault = ySensitivity;
        animator = GetComponent<Animator>();

        ///<summary>
        ///Player aim related
        /// </summary>
    }

    private void FixedUpdate()
    {
        HeadAndBodyAnimation();
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
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);
        //apply it to the camera rotation.
        playerCameraRoot.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //rotates the player to look left and right according to the face movement.
        Vector3 rotationValue = (Vector3.up * (mouseX * Time.fixedDeltaTime) * xSensitivity);
        transform.Rotate(rotationValue);
    }

    private void HeadAndBodyAnimation()
    {
        aimTarget.position = bodyHeadAimTarget.position;
    }
/// <summary>
/// End of basic look codes
/// </summary>

 ///<summary>
 ///player aiming related
 /// </summary>w
    public void PlayerAimStart()
    {
        aimCamera.Priority += aimCamPriority;
        xSensitivity = xSensitivityAim;
        ySensitivity = ySensitivityAim;
        isAiming = !isAiming;
    }
    public void PlayerAimFinished()
    {
        aimCamera.Priority -= aimCamPriority;
        xSensitivity = xSensitivityDefault;
        ySensitivity = ySensitivityDefault;
        isAiming = !isAiming;
    }
///<summary>
///end of player aiming codes
/// </summary>
}
