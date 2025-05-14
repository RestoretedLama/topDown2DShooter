using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Slider için gerekli

public class PlayerHealthSystem : MonoBehaviour
{
    public static PlayerHealthSystem Instance { get; set; }

    [Header("Health Settings")] public float maxHealth;
    public float health;
    public float armor = 0.05f;

    [Header("UI")] [SerializeField] private Slider healthBar;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        health = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = health;
        }
    }

    public void giveHealth(float heal)
    {
        health += heal;
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        float reducedDamage = damage * (1f - armor);
        health -= reducedDamage;

        Debug.Log("Gelen Hasar: " + damage + " | Azaltılmış Hasar: " + reducedDamage + " | Kalan Can: " + health);

        UpdateHealthUI();

        if (health <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.value = health;
        }
    }

    public void setMaxHealth(float h)
    {
        maxHealth = h;
        UpdateHealthUI();
    }

    public float getMaxHealth(float h)
    {
        return maxHealth;
    }

    public void setArmor(float a)
    {
        armor = a;
    }

    public float getArmor()
    {
        return armor;
    }

    public void Die()
    {
        Debug.Log("Karakter öldü.");
    }
}