using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKBehaviour : MonoBehaviour
{
#region Feet Positions and Rotations Variables
    private Vector3 rightFootPosition, leftFootPosition, leftFootIKPosition, rightFootIKPosition;
    private Quaternion leftFootIKRotation, rightFootIKRotation;
    private float lastPelvisPositionY, lastRightFootPositionY, lastLeftFootPositionY;
    #endregion

     #region FeetGrounding Variables
    [Header("Feet Grounder")]
    [SerializeField] bool enableFeetIK = true;
    [Range(-1, 2)][SerializeField] float heightFromGroundRaycast = 1.14f;
    [Range(0, 2)][SerializeField] float rayCastDownDistance = 1.5f;
    [SerializeField] LayerMask environmentLayer;
    [SerializeField] float pelvisOffset = 0f;
    [Range(0, 1)][SerializeField] float pelvisUpAndDownSpeed = 0.28f;
    [Range(0, 1)][SerializeField] float feetToIKPositionSpeed = 0.5f;
    private Animator anim;
    #endregion
    
    public bool IKRotationEnable = false;
    public bool showSolverDebug = true;

    #region Getters and Setters
    public bool EnableFeetIK { get { return enableFeetIK; } set { enableFeetIK = value; } }
    public float PelvisOffset { get { return pelvisOffset; } set { pelvisOffset = value; } }
    #endregion

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    #region FeetGrounding

    /// <summary>
    /// Updating the AdjustFeetTarget method and also finding the position of each foot inside the SolverPosition.
    /// </summary>
    private void Update()
    {
        if(enableFeetIK == false)
        {
            return;
        }
        if(anim == null)
        {
            return;
        }
        AdjustFeetTarget(ref rightFootPosition, HumanBodyBones.RightFoot);
        AdjustFeetTarget(ref leftFootPosition, HumanBodyBones.LeftFoot);

        //find and raycast to the ground to find positions
        FeetPositionSolver(rightFootPosition, ref rightFootIKPosition, ref rightFootIKRotation); //handles the solver for the right foot
        FeetPositionSolver(leftFootPosition, ref leftFootIKPosition, ref leftFootIKRotation); // handles the solver for the left foot

    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(enableFeetIK == false)
        {
            return;
        }
        if (anim == null)
        {
            return;
        }

        MovePelvisHeight();
        //IK Rotation for right foot
        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
        if(IKRotationEnable)
        {
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, anim.GetFloat("LeftFootCurve"));
        }

        MoveFeetToIKPoint(AvatarIKGoal.RightFoot, rightFootIKPosition, rightFootIKRotation, ref lastRightFootPositionY);

        //IKRotation for left foot
        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        if (IKRotationEnable)
        {
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("RightFootCurve"));
        }

        MoveFeetToIKPoint(AvatarIKGoal.LeftFoot, leftFootIKPosition, leftFootIKRotation, ref lastLeftFootPositionY);

    }
    #endregion


    #region FeetGroundingMethods
    void MoveFeetToIKPoint(AvatarIKGoal foot, Vector3 positionIKHolder, Quaternion rotationIKHolder, ref float lastFootPositionY)
    {
        Vector3 targetIKPosition = anim.GetIKPosition(foot);
        if(positionIKHolder != Vector3.zero)
        {
            targetIKPosition = transform.InverseTransformPoint(targetIKPosition);
            positionIKHolder = transform.InverseTransformPoint(positionIKHolder);

            float yVariable = Mathf.Lerp(lastFootPositionY, positionIKHolder.y, feetToIKPositionSpeed);
            targetIKPosition.y += yVariable;
            lastFootPositionY = yVariable;
            targetIKPosition = transform.TransformPoint(targetIKPosition);
            anim.SetIKRotation(foot, rotationIKHolder);
        }
        anim.SetIKPosition(foot, targetIKPosition);
    }
    private void MovePelvisHeight()
    {
        if(rightFootIKPosition == Vector3.zero || leftFootIKPosition == Vector3.zero || lastPelvisPositionY == 0)
        {
            lastPelvisPositionY = anim.bodyPosition.y;
            return;
        }
        float leftOffSetPosition = leftFootIKPosition.y - transform.position.y;
        float rightOffSetPosition = rightFootIKPosition.y - transform.position.y;

        float totalOffSet = (leftOffSetPosition < rightOffSetPosition) ? leftOffSetPosition : rightOffSetPosition;
        Vector3 newPelvisPosition = anim.bodyPosition + Vector3.up * totalOffSet;
        newPelvisPosition.y = Mathf.Lerp(lastPelvisPositionY, newPelvisPosition.y, pelvisUpAndDownSpeed);
        anim.bodyPosition = newPelvisPosition;
        lastPelvisPositionY = anim.bodyPosition.y;
    }
    private void FeetPositionSolver(Vector3 fromSkyPosition, ref Vector3 feetIKPositions, ref Quaternion feetIKRotations)
    {
        //uses raycast to locate and solve the feet position
        RaycastHit feetOutHit;

        Debug.DrawLine(fromSkyPosition, fromSkyPosition + Vector3.down * (rayCastDownDistance + heightFromGroundRaycast), Color.yellow);

        if(Physics.Raycast(fromSkyPosition, Vector3.down, out feetOutHit, rayCastDownDistance + heightFromGroundRaycast, environmentLayer))
        {
            //finds the feet IK position from the sky
            feetIKPositions = fromSkyPosition;
            feetIKPositions.y = feetOutHit.point.y + pelvisOffset;
            feetIKRotations = Quaternion.FromToRotation(Vector3.up, feetOutHit.normal) * transform.rotation;
            return;
        }
        feetIKPositions = Vector3.zero;

    }

    private void AdjustFeetTarget(ref Vector3 feetPositions, HumanBodyBones foot)
    {
        feetPositions = anim.GetBoneTransform(foot).position;
        feetPositions.y = transform.position.y + heightFromGroundRaycast;
    }
    #endregion


}
