using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBulbSwitch : Interactable
{
    [SerializeField] GameObject lightBulb;
    [SerializeField] bool lightOn = true;

    protected override void Interact()
    {
        lightOn = !lightOn;
        lightBulb.gameObject.SetActive(lightOn);
    }
}
