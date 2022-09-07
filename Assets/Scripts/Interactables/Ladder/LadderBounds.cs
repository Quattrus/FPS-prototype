using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderBounds : MonoBehaviour
{

    [SerializeField] GameObject ladder;

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            ladder.gameObject.GetComponent<Ladder>().LadderBoundsCheck();
        }
    }
}
