using UnityEngine;

public class DmgTotemBuff : MonoBehaviour
{
    [HideInInspector] public float buffAmount;
    [Tooltip("Etki yar��ap�")]
    public float radius = 3f;
    [Tooltip("Totemin sahnede kalma s�resi (sn)")]
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
