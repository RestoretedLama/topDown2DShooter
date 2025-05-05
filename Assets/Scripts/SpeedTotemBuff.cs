// SpeedTotemBuff.cs
using UnityEngine;

public class SpeedTotemBuff : MonoBehaviour
{
    [HideInInspector] public float buffAmount;
    [Tooltip("Etki yarýçapý")]
    public float radius = 3f;
    [Tooltip("Totemin sahnede kalma süresi (sn)")]
    public float duration = 5f;

    void Start()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hit in hits)
        {
            var e = hit.GetComponent<BaseEnemy>();
            var bc = hit.GetComponent<BasicEnemyController>();
            if (e != null && bc != null && !e.hasReceivedSpeedBuff)
            {
                bc.moveSpeed += buffAmount;
                e.hasReceivedSpeedBuff = true;
            }
        }
        Destroy(gameObject, duration);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
