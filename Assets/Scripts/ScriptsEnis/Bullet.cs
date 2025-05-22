using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float whiteDuration = 0.1f;
    [SerializeField ]private float autoDeactivateTime = 1.5f;
    private float damage;

    private void OnEnable()
    {
        // Otomatik geri dönüş zamanlayıcısı
        CancelInvoke();
        Invoke(nameof(Deactivate), autoDeactivateTime);
    }

    private void OnDisable()
    {
        // Obje yeniden aktif edilmeden önce zamanlayıcıyı temizle
        CancelInvoke();
    }

    public void Start()
    {
        // Bu method güvenli değilse SetDamage() kullanmanı öneririm
        damage = Inventory.instance.GetCurrentItem().damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<BaseEnemy>();
            var enemyFlash = other.GetComponent<EnemyFlash>();

            if (enemy != null)
                enemy.TakeDamage(damage);

            if (enemyFlash != null)
                enemyFlash.FlashWhite(whiteDuration);

            Deactivate();
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    // Alternatif olarak hasarı dışarıdan set etmek istersen:
    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
}