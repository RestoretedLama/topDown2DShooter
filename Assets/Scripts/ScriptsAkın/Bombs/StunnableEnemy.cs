using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SpriteRenderer))]
public class StunnableEnemy : MonoBehaviour, IDamageable, IStunnable
{
    [Header("Enemy Stats")]
    [SerializeField] private int health = 100;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int attackDamage = 10;
    
    [Header("Stun Effects")]
    [SerializeField] private GameObject stunEffectPrefab;
    [SerializeField] private Color stunColor = new Color(0.7f, 0.7f, 1.0f);
    
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private NavMeshAgent agent; // Optional: for enemies that use navmesh
    private Coroutine stunCoroutine;
    private Rigidbody2D rb;
    private GameObject activeStunEffect;
    
    public bool IsStunned { get; private set; }
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody2D>();
        
        // Optional: find player automatically
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
    
    private void Update()
    {
        if (IsStunned || player == null)
            return;
            
        // Implement enemy AI logic here when not stunned
        // This is just a simple example - move towards player
        if (agent != null)
        {
            agent.SetDestination(player.position);
        }
        else if (rb != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            // Simple movement without physics
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        
        // Flash the sprite to indicate damage
        StartCoroutine(DamageFlash());
        
        if (health <= 0)
        {
            Die();
        }
    }
    
    private IEnumerator DamageFlash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        
        if (!IsStunned) // Don't override stun color
        {
            spriteRenderer.color = originalColor;
        }
    }
    
    private void Die()
    {
        // Play death animation, drop loot, etc.
        Destroy(gameObject);
    }
    
    public void Stun(float duration)
    {
        // Don't allow stacking stuns, but refresh duration
        if (stunCoroutine != null)
        {
            StopCoroutine(stunCoroutine);
        }
        
        stunCoroutine = StartCoroutine(StunCoroutine(duration));
    }
    
    private IEnumerator StunCoroutine(float duration)
    {
        IsStunned = true;
        
        // Disable movement
        if (agent != null) agent.isStopped = true;
        if (rb != null) rb.velocity = Vector2.zero;
        
        // Visual indicators
        spriteRenderer.color = stunColor;
        
        // Create stun effect (stars circling around head)
        if (stunEffectPrefab != null && activeStunEffect == null)
        {
            activeStunEffect = Instantiate(stunEffectPrefab, transform);
            activeStunEffect.transform.localPosition = Vector3.up * 0.5f; // Position above head
        }
        
        // Wait for stun duration
        yield return new WaitForSeconds(duration);
        
        // Re-enable movement
        if (agent != null) agent.isStopped = false;
        
        // Remove visual effects
        spriteRenderer.color = originalColor;
        if (activeStunEffect != null)
        {
            Destroy(activeStunEffect);
            activeStunEffect = null;
        }
        
        IsStunned = false;
        stunCoroutine = null;
    }
} 