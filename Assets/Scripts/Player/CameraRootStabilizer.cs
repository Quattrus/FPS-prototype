using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRootStabilizer : MonoBehaviour
{
    [SerializeField] Transform playerArmatureNeck;
    [SerializeField] float snapSpeed;

    void Update()
    {
        transform.position = playerArmatureNeck.position;
    }
}
