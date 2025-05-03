using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float whiteDuration = 0.1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyFlash enemyFlash = collision.GetComponent<EnemyFlash>();
            if (enemyFlash != null)
            {
                enemyFlash.FlashWhite(whiteDuration);
            }

            Destroy(gameObject); // Bullet yok olur ama enemy coroutine çalıştırmaya devam eder
        }
    }
}