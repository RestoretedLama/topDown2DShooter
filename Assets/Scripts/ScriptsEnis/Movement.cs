using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private Vector2 movement;
    private bool isDashing = false;
    private bool canDash = true;

    void Update()
    {
        if (!isDashing)
        {
            move();
            if (Input.GetKeyDown(KeyCode.Space) && canDash && movement != Vector2.zero)
            {
                StartCoroutine(Dash());
            }
        }
    }

    private IEnumerator Dash()
    {
        WeaponController.instance.setCanShoot(false);
        isDashing = true;
        canDash = false;

        Vector2 dashDirection = movement.normalized;

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            rb.velocity = dashDirection * dashSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        WeaponController.instance.setCanShoot(true);
    }

    public float setMoveSpeed(float speed)
    {
        return moveSpeed = speed;
    }

    private void move()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        rb.velocity = movement.normalized * moveSpeed;
    }
}