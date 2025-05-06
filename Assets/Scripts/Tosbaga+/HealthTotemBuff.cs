using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CircleCollider2D))]
public class HealthTotemBuff : MonoBehaviour
{
    [Tooltip("Saniyede ka� birim heal versin")]
    public float healPerSecond = 10f;
    [Tooltip("Totemin aktif kalma s�resi (sn)")]
    public float duration = 5f;

    private CircleCollider2D trigger;
    private readonly List<BaseEnemy> inside = new();

    void Awake()
    {
        trigger = GetComponent<CircleCollider2D>();
        trigger.isTrigger = true;
    }

    void Start()
    {
        // Belirtilen s�re sonra totemi yok et
        Destroy(gameObject, duration);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var e = col.GetComponent<BaseEnemy>();
        if (e != null && !inside.Contains(e))
            inside.Add(e);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        var e = col.GetComponent<BaseEnemy>();
        if (e != null)
            inside.Remove(e);
    }

    void Update()
    {
        // ��erideki her d��mana saniye ba��na heal uygula
        float healThisFrame = healPerSecond * Time.deltaTime;
        foreach (var e in inside)
        {
            e.currentHealth = Mathf.Min(e.currentHealth + healThisFrame, e.maxHealth);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (trigger == null) trigger = GetComponent<CircleCollider2D>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trigger.radius);
    }
}
