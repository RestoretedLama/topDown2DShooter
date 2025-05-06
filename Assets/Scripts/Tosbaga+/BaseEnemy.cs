using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BaseEnemy : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Düşmanın maksimum canı")]
    public float maxHealth = 100f;
    [Tooltip("Düşmanın güncel canı")]
    public float currentHealth;

    [Tooltip("Düşmanın vurduğu hasar")]
    public float damage = 10f;

    [Header("Buff Flags")]

    protected SpriteRenderer sprite;

    protected virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
            Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
