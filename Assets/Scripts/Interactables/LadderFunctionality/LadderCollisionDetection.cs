using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderCollisionDetection : MonoBehaviour
{
    [SerializeField] GameObject ladder;
    private void OnTriggerEnter(Collider other)
    {
        if(gameObject.tag == "BottomLadderCheck" && other.gameObject.tag == "Player")
        {
            Debug.Log("bottomCollision");
            ladder.gameObject.GetComponent<Ladder>()._canExit = true;
        }
        else if(gameObject.tag == "TopLadderCheck" && other.gameObject.tag == "Player")
        {
            Debug.Log("topCollision");
        }
    }
}
