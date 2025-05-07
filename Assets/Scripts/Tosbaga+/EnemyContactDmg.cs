using UnityEngine;

[RequireComponent(typeof(BaseEnemy), typeof(Collider2D))]
public class EnemyContactDamage : MonoBehaviour
{
    [Tooltip("Ýki saldýrý arasý minimum bekleme süresi (sn)")]
    public float attackInterval = 1f;

    private BaseEnemy be;
    private float nextAttackTime;
    private Collider2D col;

    void Awake()
    {
        be = GetComponent<BaseEnemy>();
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // “Player” tag’li objeye saldýr
        if (other.CompareTag("Player") && Time.time >= nextAttackTime)
        {
            // be.damage deðerini PlayerHealthSystem’e yolla
            PlayerHealthSystem.Instance.TakeDamage(be.damage);
            nextAttackTime = Time.time + attackInterval;
        }
    }
}
