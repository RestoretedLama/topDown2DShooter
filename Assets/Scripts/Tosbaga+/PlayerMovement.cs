using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Her karede tu� durumunu kontrol edip movement vekt�r�n� olu�tur
        movement = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            movement.y = +1f;
        if (Input.GetKey(KeyCode.S))
            movement.y = -1f;
        if (Input.GetKey(KeyCode.A))
            movement.x = -1f;
        if (Input.GetKey(KeyCode.D))
            movement.x = +1f;

        // Diyagonal hareket h�z�n� e�itle
        movement = movement.normalized;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }


}