using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : Interactable
{
    #region handles adding ammo value and destroy ammo game object 
    protected override void Interact()
    {
        Inventory.Instance.AmmoAdded();
        Destroy(gameObject);
    }
    #endregion
}
