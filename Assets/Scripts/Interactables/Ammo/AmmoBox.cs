using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : Interactable
{
    [SerializeField] GameObject gun;

    protected override void Interact()
    {
        gun.gameObject.GetComponent<Gun>().AmmoAdded();
        Destroy(gameObject);
    }
}
