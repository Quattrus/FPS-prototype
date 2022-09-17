using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthController : MonoBehaviour
{
    [Header("Health Main Parameters")]
    [SerializeField] float playerHealth = 100f;
    [SerializeField] float maxHealth = 100f;

    [Header("Health Value Parameters")]
    [SerializeField] float healingValue;


    [Header("Health UI Elements")]
    [SerializeField] Image healthBarUI = null;
    [SerializeField] CanvasGroup healthSliderCanvasGroup = null;

    private void Update()
    {
        healthBarUI.fillAmount = playerHealth / maxHealth;
    }

    public void DamagePlayer(int damageAmount)
    {
        playerHealth -= damageAmount;
        healthBarUI.fillAmount = playerHealth / damageAmount;
    }
    public void PlayerHeal(int healAmount)
    {
        playerHealth += healAmount;
        healthBarUI.fillAmount = playerHealth / maxHealth;
    }    
    
}
