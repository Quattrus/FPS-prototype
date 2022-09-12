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

    private Inventory inventory;
    [SerializeField] GameObject gun;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

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
       int ammoRemaining = inventory.AvailableClips;
       ammoClipsText.text = ("Ammo Clips: " + ammoRemaining);
    }

    public void RemainingBulletsCheck()
    {
        if(gun.gameObject != null)
        {
            if (gun.gameObject.GetComponent<Gun>().isReloading)
            {
                remainingBulletsText.text = ("Reloading");
            }
            else
            {
                int bulletsRemaining = inventory.CurrentAmmo;
                remainingBulletsText.text = ("Bullets: " + bulletsRemaining);
            }
        }

    }
}
