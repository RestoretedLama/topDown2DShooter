using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;
    private int damage = 10; // Default damage, will be overridden
    private int shooterLayerMask; // To store the shooter's layer

    private void Start()
    {
        // Destroy the bullet after lifetime seconds
        Destroy(gameObject, lifetime);
    }

    // Set damage from weapon
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
    
    // Set the layer of the shooter to ignore collisions
    public void SetShooterLayer(int layer)
    {
        shooterLayerMask = 1 << layer;
        
        // Optionally, you can set the bullet to ignore collisions with the shooter immediately
        Physics2D.IgnoreLayerCollision(gameObject.layer, layer, true);
    }

    // Handle collision with enemies or other objects
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Skip collision if it's the shooter or same layer as shooter
        if ((shooterLayerMask & (1 << collision.gameObject.layer)) != 0)
        {
            return;
        }
        
        // Check if we hit an enemy or damageable object
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }

        // Destroy bullet on impact (except with trigger layers if needed)
        if (!collision.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}