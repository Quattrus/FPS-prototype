using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerUI : MonoBehaviour
{
    [Header("All UI Prompts goes here")]
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private TextMeshProUGUI ammoClipsText;
    [SerializeField] private TextMeshProUGUI remainingBulletsText;

    [SerializeField] GameObject gun;

    public void FixedUpdate()
    {
        AmmoClipsCheck();
        RemainingBulletsCheck();
    }


    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }

    public void AmmoClipsCheck()
    {
        int ammoRemaining = gun.gameObject.GetComponent<Gun>().availableClips;
        ammoClipsText.text = ("Ammo Clips: " + ammoRemaining);
    }

    public void RemainingBulletsCheck()
    {
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
}
