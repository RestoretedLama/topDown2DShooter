using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireZone : MonoBehaviour
{
    [SerializeField] private float tickRate = 0.5f;
    
    private int damagePerTick;
    private float duration;
    private float radius;
    private float elapsedTime = 0f;
    
    private List<IDamageable> entitiesInZone = new List<IDamageable>();
    
    public void Initialize(int damage, float lifetime, float effectRadius)
    {
        damagePerTick = damage;
        duration = lifetime;
        radius = effectRadius;
        
        // Scale the fire zone to match the radius
        transform.localScale = new Vector3(radius * 2, radius * 2, 1);
        
        // Start damage ticks
        StartCoroutine(DamageTick());
        
        // Destroy after duration
        Destroy(gameObject, duration);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null && !entitiesInZone.Contains(damageable))
        {
            entitiesInZone.Add(damageable);
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            entitiesInZone.Remove(damageable);
        }
    }
    
    private IEnumerator DamageTick()
    {
        while (elapsedTime < duration)
        {
            yield return new WaitForSeconds(tickRate);
            elapsedTime += tickRate;
            
            // Apply damage to all entities in the fire zone
            foreach (var entity in new List<IDamageable>(entitiesInZone))
            {
                if (entity != null)
                {
                    entity.TakeDamage(damagePerTick);
                }
            }
            
            // Clean up null references
            entitiesInZone.RemoveAll(item => item == null);
        }
    }
}