// DmgTotemBuff.cs
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CircleCollider2D))]
public class DmgTotemBuff : MonoBehaviour
{
    [Tooltip("Eklenecek hasar miktarý")]
    public float dmgBuffAmount = 10f;
    [Tooltip("Totemin sahnede kalma süresi (sn)")]
    public float duration = 5f;

    private CircleCollider2D trigger;
    private readonly List<BaseEnemy> affected = new();

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
        var e = col.GetComponent<BaseEnemy>();
        if (e != null && !affected.Contains(e))
        {
            e.damage += dmgBuffAmount;
            affected.Add(e);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        var e = col.GetComponent<BaseEnemy>();
        if (e != null && affected.Remove(e))
        {
            e.damage -= dmgBuffAmount;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, GetComponent<CircleCollider2D>().radius);
    }
}
