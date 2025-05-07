using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    public static PlayerHealthSystem Instance { get; set;}
    public float maxHealth;
    public float health;
    public float armor = 0.05f;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        health = maxHealth;
    }

    public  void giveHealth(float heal)
    {
        health = heal;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
    public void TakeDamage(float damage)
    {
        float reducedDamage = damage * (1f - armor);
        health -= reducedDamage;

        Debug.Log("Gelen Hasar: " + damage + " | Azaltılmış Hasar: " + reducedDamage + " | Kalan Can: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Karakter öldü.");
    }
}
