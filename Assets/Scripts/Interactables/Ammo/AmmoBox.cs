using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : Interactable
{
    [SerializeField] GameObject player;

    protected override void Interact()
    {
        player.gameObject.GetComponent<Inventory>().AmmoAdded();
        Destroy(gameObject);
    }
}
