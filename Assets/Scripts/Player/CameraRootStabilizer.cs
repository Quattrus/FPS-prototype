using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRootStabilizer : MonoBehaviour
{
    [SerializeField] Transform playerArmatureNeck;

    void FixedUpdate()
    {
        transform.position = playerArmatureNeck.position;
    }
}
