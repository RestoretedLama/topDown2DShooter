using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasicEnemyController : MonoBehaviour
{
    [Header("Yürüme Hýzý")]
    public float moveSpeed = 2f;

    Rigidbody2D rb;
    Transform player;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        var p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null) return;
        // Oyuncuya doðru vektör oluþtur
        Vector2 dir = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
    }
}
