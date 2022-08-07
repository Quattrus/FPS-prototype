using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Target : MonoBehaviour
{
    public void AttackTarget()
    {
        AttackThisTarget();
    }
    protected virtual void AttackThisTarget()
    {

    }
}
