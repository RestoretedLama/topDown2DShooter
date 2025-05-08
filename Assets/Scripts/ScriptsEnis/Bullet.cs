using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float whiteDuration = 0.1f;
    private float damage;
    public void Start()
    {
        damage = Inventory.instance.GetCurrentItem().damage;
    }
    // Efektin kaç saniye sonra yok olacağı

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyFlash enemyFlash = other.GetComponent<EnemyFlash>();
            other.GetComponent<BaseEnemy>().TakeDamage(damage);
            if (enemyFlash != null)
            {
                enemyFlash.FlashWhite(whiteDuration);
            }

            Destroy(gameObject);
        }
        
    }

}
