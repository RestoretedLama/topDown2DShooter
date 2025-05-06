using UnityEngine;

public class ArcherEnemy : BaseEnemy
{
    [Header("Archer Properties")]
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public float attackRange = 10f;
    public float attackCooldown = 2f;
    private float attackTimer;

    private Transform target;

    protected override void Awake()
    {
        base.Awake();
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);
        if (distance <= attackRange && attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackCooldown;
        }

        attackTimer -= Time.deltaTime;
    }

    protected virtual void Attack()
    {
        if (arrowPrefab != null && shootPoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
            Vector2 direction = (target.position - shootPoint.position).normalized;

            // Örneğin, Rigidbody2D ile bir kuvvet uygulayalım
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * 10f; // ok hızı
            }
        }
    }
}
