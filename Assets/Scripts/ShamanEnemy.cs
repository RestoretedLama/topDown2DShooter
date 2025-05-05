using UnityEngine;

[RequireComponent(typeof(BaseEnemy), typeof(BasicEnemyController))]
public class ShamanEnemy : MonoBehaviour
{
    [Header("Totem Prefab’leri")]
    public GameObject healthTotemPrefab;
    public GameObject speedTotemPrefab;
    public GameObject dmgTotemPrefab;

    [Header("Cooldown ve Ofset")]
    [Tooltip("Her kaç saniyede bir totem atılsın")]
    public float totemCooldown = 10f;
    [Tooltip("İlk totem atma gecikmesi (sn)")]
    public float initialDelay = 5f;
    [Tooltip("Totem spawn pozisyonu için X ofseti")]
    public float totemOffsetX = 1f;

    [Header("Buff Miktarları")]
    public float healthBuffAmount = 50f;
    public float speedBuffAmount = 2f;   
    public float dmgBuffAmount = 10f;  

    private float nextTotemTime;

    void Start()
    {
        // İlk buff'u initialDelay sn sonra at
        nextTotemTime = Time.time + initialDelay;
    }

    void Update()
    {
        if (Time.time >= nextTotemTime)
        {
            SpawnRandomTotem();
            nextTotemTime = Time.time + totemCooldown;
        }
    }

    void SpawnRandomTotem()
    {
        // 0 → Health, 1 → Speed, 2 → Damage
        int idx = Random.Range(0, 3);

        GameObject prefab = null;
        float buffAmt = 0f;

        switch (idx)
        {
            case 0:
                prefab = healthTotemPrefab;
                buffAmt = healthBuffAmount;
                break;
            case 1:
                prefab = speedTotemPrefab;
                buffAmt = speedBuffAmount;
                break;
            case 2:
                prefab = dmgTotemPrefab;
                buffAmt = dmgBuffAmount;
                break;
        }

        if (prefab == null) return;

        Vector3 spawnPos = transform.position + Vector3.right * totemOffsetX;
        GameObject totem = Instantiate(prefab, spawnPos, Quaternion.identity);

        // İlgili TotemBuff script'ine miktarı ilet
        if (idx == 0)
        {
            var buff = totem.GetComponent<HealthTotemBuff>();
            if (buff != null) buff.buffAmount = buffAmt;
        }
        else if (idx == 1)
        {
            var buff = totem.GetComponent<SpeedTotemBuff>();
            if (buff != null) buff.buffAmount = buffAmt;
        }
        else 
        {
            var buff = totem.GetComponent<DmgTotemBuff>();
            if (buff != null) buff.buffAmount = buffAmt;
        }
    }
}
