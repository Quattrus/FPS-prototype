using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StaminaController : MonoBehaviour
{
    [Header("Stamina Main Parameters")]
    [SerializeField] float playerStamina = 100.0f;
    [SerializeField] float maxStamina = 100.0f;
    [SerializeField] float jumpCost = 20f;
    [SerializeField] bool hasRegenerated = true;

    [Header("Stamina Regen Parameters")]
    [Range(0, 50)][SerializeField] float staminaDrain = 1f;
    [Range(0, 50)][SerializeField] float staminaRegen = 0.5f;

    [Header("Staina UI Elements")]
    [SerializeField] private Image staminaProgressUI = null;
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;

    private PlayerStateMachine playerStateMachine;


    private void Awake()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void Update()
    {
        if(!playerStateMachine.IsSprinting)
        {
            if(playerStamina <= maxStamina - 0.01f)
            {
                staminaProgressUI.fillAmount = playerStamina / maxStamina;
                playerStamina += staminaRegen * Time.deltaTime;
            }
        }
    }

    public void Sprinting()
    {
        if(hasRegenerated)
        {
            playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina(1);
        }
        if(playerStamina <= 0)
        {
            hasRegenerated = false;
            playerStateMachine.IsSprinting = false;
        }
    }

    public void StaminaJump()
    {
        if(playerStamina >= (maxStamina * jumpCost / maxStamina))
        {
            playerStamina -= jumpCost;
            UpdateStamina(1);
        }
    }

    private void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;
        if(value == 0)
        {
            sliderCanvasGroup.alpha = 0;
        }
        else
        {
            sliderCanvasGroup.alpha = 1;
        }
    }

}
