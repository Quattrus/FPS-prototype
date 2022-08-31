using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCheck : MonoBehaviour
{
    [SerializeField] LayerMask WallCheckLayerMask;
    [SerializeField] Transform debugTransform;
    private float distance;



    public void WallCheck()
    {
        RaycastHit checkHit = new RaycastHit();
        Vector3 origin = this.transform.position;
        Debug.DrawRay(origin, transform.forward, Color.red);
        if (Physics.Raycast(origin, transform.forward, out checkHit, 5f, WallCheckLayerMask))
        { 
            debugTransform.transform.position = checkHit.point;
            distance = Vector3.Distance(checkHit.point, origin);
            Debug.Log("Distance to wall is: " + distance);
        }
    }
    

}
