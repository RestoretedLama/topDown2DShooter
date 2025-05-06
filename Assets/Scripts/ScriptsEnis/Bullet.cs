using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float whiteDuration = 0.1f;
    public GameObject hitEffectPrefab; // Inspector'dan atayacağın efekt prefabı
    public float effectLifetime = 1f;   // Efektin kaç saniye sonra yok olacağı

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyFlash enemyFlash = collision.GetComponent<EnemyFlash>();
            BaseEnemy be = GetComponent<BaseEnemy>();
            be.TakeDamage(50);
            if (enemyFlash != null)
            {
                enemyFlash.FlashWhite(whiteDuration);
            }

            // Vuruş efektini oluştur
            /*
            if (hitEffectPrefab != null)
            {
                GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, effectLifetime); // Efekti bir süre sonra yok et
            }*/

            Destroy(gameObject); // Mermiyi yok et
        }
        
    }
    
}
