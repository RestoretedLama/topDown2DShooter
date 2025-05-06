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
    [HideInInspector] public bool hasReceivedHealthBuff;
    [HideInInspector] public bool hasReceivedSpeedBuff;
    [HideInInspector] public bool hasReceivedDamageBuff;

    protected SpriteRenderer sprite;

    void OnValidate()
    {
        currentHealth = maxHealth;
        hasReceivedHealthBuff = false;
        hasReceivedSpeedBuff = false;
        hasReceivedDamageBuff = false;
    }

    protected virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        hasReceivedHealthBuff = false;
        hasReceivedSpeedBuff = false;
        hasReceivedDamageBuff = false;
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
