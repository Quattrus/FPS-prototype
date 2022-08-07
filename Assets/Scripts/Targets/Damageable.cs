using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] GameObject hitEffects;
    private float currentHealth;


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage, Vector3 hitPos, Vector3 hitNormal)
    {
        Instantiate(hitEffects, hitPos, Quaternion.LookRotation(hitNormal));
        currentHealth -= damage;
        if(currentHealth < 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
