using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasicEnemyController : MonoBehaviour
{
    [Header("Y�r�me H�z�")]
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
        // Oyuncuya do�ru vekt�r olu�tur
        Vector2 dir = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
    }
}
