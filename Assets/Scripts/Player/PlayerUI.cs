using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI remainingBulletsText;

    [SerializeField] GameObject gun;

    public void FixedUpdate()
    {
        int ammoRemaining = gun.gameObject.GetComponent<Gun>().availableClips;
        ammoText.text = ("Ammo Clips: " + ammoRemaining);
        if (gun.gameObject.GetComponent<Gun>().isReloading)
        {
            remainingBulletsText.text = ("Reloading");
        }
        else
        {
            int bulletsRemaining = gun.gameObject.GetComponent<Gun>().currentAmmo;
            remainingBulletsText.text = ("Bullets: " + bulletsRemaining);
        }
        
        
        

    }


    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
