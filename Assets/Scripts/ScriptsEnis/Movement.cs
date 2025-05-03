using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]private float moveSpeed = 5f;
    [SerializeField]private Rigidbody2D rb;
    Vector2 movement;

    void Update()
    {
        
        Move();
    }

    private void Move()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    public float setMoveSpeed(float speed)
    {
        return moveSpeed = speed;
    }
}
