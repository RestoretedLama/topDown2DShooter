using UnityEngine;

public class HealthTotemBuff : MonoBehaviour
{
    [HideInInspector] public float buffAmount;
    [Tooltip("Etki yar��ap�")]
    public float radius = 3f;
    [Tooltip("Totemin sahnede kalma s�resi (sn)")]
    public float duration = 5f;

    void Start()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hit in hits)
        {
            var e = hit.GetComponent<BaseEnemy>();
            if (e != null && !e.hasReceivedHealthBuff)
            {
                e.maxHealth += buffAmount;
                e.currentHealth += buffAmount;
                e.hasReceivedHealthBuff = true;
            }
        }
        Destroy(gameObject, duration);
    }
    void OnDrawGizmosSelected()
    {
        // Test i�in radius g�ster
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
