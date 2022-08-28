using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRootStabilizer : MonoBehaviour
{
    [SerializeField] Transform cameraRootFollow;

    void LateUpdate()
    {
        transform.position = cameraRootFollow.position;
    }
}
