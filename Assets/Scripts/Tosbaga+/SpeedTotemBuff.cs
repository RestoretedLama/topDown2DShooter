// SpeedTotemBuff.cs
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CircleCollider2D))]
public class SpeedTotemBuff : MonoBehaviour
{
    [Tooltip("Eklenecek hýz miktarý")]
    public float speedBuffAmount = 2f;
    [Tooltip("Totemin sahnede kalma süresi (sn)")]
    public float duration = 5f;

    private CircleCollider2D trigger;
    private readonly List<BasicEnemyController> affected = new();

    void Awake()
    {
        trigger = GetComponent<CircleCollider2D>();
        trigger.isTrigger = true;
    }

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var bc = col.GetComponent<BasicEnemyController>();
        if (bc != null && !affected.Contains(bc))
        {
            bc.moveSpeed += speedBuffAmount;
            affected.Add(bc);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        var bc = col.GetComponent<BasicEnemyController>();
        if (bc != null && affected.Remove(bc))
        {
            bc.moveSpeed -= speedBuffAmount;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, GetComponent<CircleCollider2D>().radius);
    }
}
