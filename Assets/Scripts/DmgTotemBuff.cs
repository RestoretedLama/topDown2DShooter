using UnityEngine;

public class DmgTotemBuff : MonoBehaviour
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
            if (e != null && !e.hasReceivedDamageBuff)
            {
                e.damage += buffAmount;
                e.hasReceivedDamageBuff = true;
            }
        }
        Destroy(gameObject, duration);
    }
}
