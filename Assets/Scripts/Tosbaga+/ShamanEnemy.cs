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
    [Tooltip("HealthTotem’in saniyede ne kadar heal vereceği")]
    public float healthBuffAmount = 10f;
    [Tooltip("SpeedTotem’in eklenecek hız miktarı")]
    public float speedBuffAmount = 2f;
    [Tooltip("DmgTotem’in eklenecek hasar miktarı")]
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

        // İlgili TotemBuff script’ine doğru alanı ayarla
        if (idx == 0)
        {
            var healthBuff = totem.GetComponent<HealthTotemBuff>();
            if (healthBuff != null)
                healthBuff.healPerSecond = buffAmt;
        }
        else if (idx == 1)
        {
            var speedBuff = totem.GetComponent<SpeedTotemBuff>();
            if (speedBuff != null)
                speedBuff.speedBuffAmount = buffAmt;
        }
        else // idx == 2
        {
            var dmgBuff = totem.GetComponent<DmgTotemBuff>();
            if (dmgBuff != null)
                dmgBuff.dmgBuffAmount = buffAmt;
        }
    }
}
