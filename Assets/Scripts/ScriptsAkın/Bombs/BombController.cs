using System.Collections;
using UnityEngine;
using Weapons.Bombs;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BombController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject fireZonePrefab; // For Molotov
    [SerializeField] private ParticleSystem fuseParticles;
    [SerializeField] private AudioSource fuseAudio;
    [SerializeField] private AudioSource explosionAudio;
    
    [Header("Throw Settings")]
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float arcFactor = 0.5f;
    
    // Bomb properties (set from ScriptableObject)
    private BombType bombType;
    private float fuseTime;
    private float blastRadius;
    private int damage;
    private float dotDuration;
    private float blindDuration; // Now used as stun duration for enemies
    
    private Rigidbody2D rb;
    private bool hasExploded = false;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    public void Initialize(Item bombItem)
    {
        if (bombItem.itemType != ItemType.Bomb)
        {
            Debug.LogError("Attempted to initialize BombController with non-bomb item");
            return;
        }
        
        // Get properties directly from Item
        bombType = bombItem.bombType;
        fuseTime = bombItem.fuseTime;
        blastRadius = bombItem.blastRadius;
        damage = bombItem.bombDamage;
        dotDuration = bombItem.dotDuration;
        blindDuration = bombItem.blindDuration;
        
        // Start fuse timer
        StartCoroutine(FuseTimer());
    }
    
    public void Throw(Vector2 direction)
    {
        // Apply throw force with arc
        Vector2 force = direction.normalized * throwForce;
        force.y += arcFactor * throwForce;
        rb.AddForce(force, ForceMode2D.Impulse);
        
        // Start fuse effects
        if (fuseParticles) fuseParticles.Play();
        if (fuseAudio) fuseAudio.Play();
    }
    
    private IEnumerator FuseTimer()
    {
        yield return new WaitForSeconds(fuseTime);
        Explode();
    }
    
    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;
        
        // Stop fuse effects
        if (fuseParticles) fuseParticles.Stop();
        if (fuseAudio) fuseAudio.Stop();
        
        // Play explosion sound
        if (explosionAudio) explosionAudio.Play();
        
        // Create explosion visual
        if (explosionPrefab)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        
        // Handle explosion based on bomb type
        switch (bombType)
        {
            case BombType.Frag:
                HandleFragExplosion();
                break;
            case BombType.Flash:
                HandleFlashExplosion();
                break;
            case BombType.Molotov:
                HandleMolotovExplosion();
                break;
        }
        
        // Destroy bomb (after explosion audio finishes if it exists)
        float destroyDelay = explosionAudio ? explosionAudio.clip.length : 0.1f;
        Destroy(gameObject, destroyDelay);
    }
    
    private void HandleFragExplosion()
    {
        // Damage all damageable objects within blast radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, blastRadius);
        foreach (var hitCollider in hitColliders)
        {
            IDamageable damageable = hitCollider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                // Calculate falloff damage based on distance
                float distance = Vector2.Distance(transform.position, hitCollider.transform.position);
                float damageMultiplier = 1 - (distance / blastRadius);
                int actualDamage = Mathf.RoundToInt(damage * damageMultiplier);
                damageable.TakeDamage(Mathf.Max(1, actualDamage));
            }
            
            // Optional: Add force to rigidbodies
            Rigidbody2D rb = hitCollider.GetComponent<Rigidbody2D>();
            if (rb != null && !rb.isKinematic)
            {
                Vector2 direction = (hitCollider.transform.position - transform.position).normalized;
                float force = (1 - (Vector2.Distance(transform.position, hitCollider.transform.position) / blastRadius)) * throwForce * 2;
                rb.AddForce(direction * force, ForceMode2D.Impulse);
            }
        }
    }
    
    private void HandleFlashExplosion()
    {
        // Find all enemies in range and apply stun effect
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, blastRadius);
        foreach (var hitCollider in hitColliders)
        {
            // Check if it's an enemy (using tag or layer comparison)
            if (hitCollider.CompareTag("Enemy") || hitCollider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                // Calculate stun duration based on distance
                float distance = Vector2.Distance(transform.position, hitCollider.transform.position);
                float intensityMultiplier = 1 - (distance / blastRadius);
                float stunDuration = blindDuration * intensityMultiplier;
                
                // Apply stun effect
                IStunnable stunnable = hitCollider.GetComponent<IStunnable>();
                if (stunnable != null)
                {
                    stunnable.Stun(stunDuration);
                }
                else
                {
                    // If the enemy doesn't implement IStunnable, try to find an enemy controller to stun
                    var enemyController = hitCollider.GetComponent<MonoBehaviour>();
                    if (enemyController != null)
                    {
                        // Add a visual indicator for stunned state
                        GameObject stunEffect = new GameObject("StunEffect");
                        stunEffect.transform.SetParent(hitCollider.transform);
                        stunEffect.transform.localPosition = Vector3.up; // Above the enemy
                        
                        // Add a sprite renderer with stun stars or similar visual
                        SpriteRenderer stunRenderer = stunEffect.AddComponent<SpriteRenderer>();
                        // Set sprite here if you have one assigned
                        
                        // Destroy the visual effect after stun duration
                        Destroy(stunEffect, stunDuration);
                        
                        Debug.Log($"Stunned enemy {hitCollider.name} for {stunDuration} seconds");
                    }
                }
            }
        }
    }
    
    private void HandleMolotovExplosion()
    {
        // Create fire zone
        if (fireZonePrefab != null)
        {
            GameObject fireZone = Instantiate(fireZonePrefab, transform.position, Quaternion.identity);
            FireZone fireZoneComponent = fireZone.GetComponent<FireZone>();
            if (fireZoneComponent != null)
            {
                fireZoneComponent.Initialize(damage, dotDuration, blastRadius);
            }
            else
            {
                Debug.LogWarning("FireZone component missing from fireZonePrefab");
                Destroy(fireZone, dotDuration);
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        // Draw blast radius in editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}