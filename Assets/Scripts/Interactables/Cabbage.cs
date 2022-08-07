using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabbage : Interactable
{
    [SerializeField] GameObject cabbageDirtParticle;


    protected override void Interact()
    {
        Destroy(gameObject);
        Instantiate(cabbageDirtParticle, transform.position, Quaternion.identity);
    }
}
