using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHeightCheck : MonoBehaviour
{
    [SerializeField] LayerMask groundCheckLayerMask;
    [SerializeField] Transform debugTransform;
    private float groundDistance;

    private void FixedUpdate()
    {
        WallCheck();
    }

    private void WallCheck()
    {
        RaycastHit checkHit = new RaycastHit();
        Vector3 origin = this.transform.position;
        Debug.DrawRay(origin, -transform.up, Color.red);
        if (Physics.Raycast(origin, -transform.up, out checkHit, 5f, groundCheckLayerMask))
        {

            debugTransform.transform.position = checkHit.point;
            groundDistance = Vector3.Distance(checkHit.point, origin);
            Debug.Log("distance to ground is:" + groundDistance);
        }
    }
}
