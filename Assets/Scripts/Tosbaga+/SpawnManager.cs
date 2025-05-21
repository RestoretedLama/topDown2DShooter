using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Noktalarý ve Prefab")]
    public List<Transform> spawnPoints;
    public GameObject zombiePrefab;

    [Header("Dalga Ayarlarý")]
    [Tooltip("Bu liste her dalga için spawnlanacak zombi sayýsýný tutar.")]
    public List<int> zombiesPerWave = new List<int> { 5, 8, 12 };
    [Tooltip("Dalgalar arasý bekleme süresi (sn)")]
    public float timeBetweenWaves = 5f;
    [Tooltip("Her bir zombi spawný arasý bekleme (sn)")]
    public float spawnInterval = 1f;

    private int currentWave = 0;

    void Start()
    {
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        // wavesCount kadar döngü
        int wavesCount = zombiesPerWave.Count;
        for (currentWave = 1; currentWave <= wavesCount; currentWave++)
        {
            int toSpawn = zombiesPerWave[currentWave - 1];
            Debug.Log($"<color=yellow>Wave {currentWave}/{wavesCount} baþladý: {toSpawn} zombi spawnlanacak.</color>");

            // Bu dalgadaki zombileri sýrasýyla spawnla
            for (int i = 0; i < toSpawn; i++)
            {
                SpawnZombie();
                yield return new WaitForSeconds(spawnInterval);
            }

            // Tüm zombiler öldüðünde bir sonraki dalgaya geç
            yield return new WaitUntil(() => FindObjectsOfType<BaseEnemy>().Length == 0);

            Debug.Log($"<color=lime>Wave {currentWave} tamamlandý!</color>");
            // Dalga sonu beklemesi (son dalgadansa level tamamlanacak)
            if (currentWave < wavesCount)
                yield return new WaitForSeconds(timeBetweenWaves);
        }

        OnLevelComplete();
    }

    private void SpawnZombie()
    {
        if (spawnPoints.Count == 0 || zombiePrefab == null) return;
        int idx = Random.Range(0, spawnPoints.Count);
        Instantiate(zombiePrefab, spawnPoints[idx].position, Quaternion.identity);
    }

    private void OnLevelComplete()
    {
        Debug.Log("<color=cyan>Level tamamlandý!</color>");
        // Burada sahne geçiþi, skor ekraný vs. yapabilirsin.
    }
}
