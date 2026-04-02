using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class WaveEntry
    {
        public GameObject enemyPrefab;
        public int count = 1;
    }

    [System.Serializable]
    public class WaveDefinition
    {
        public string waveName;
        public List<WaveEntry> entries = new List<WaveEntry>();
    }

    [Header("References")]
    public Transform enemyRoot;
    public Transform[] spawnPoints;

    [Header("Wave Data")]
    public List<WaveDefinition> waves = new List<WaveDefinition>();

    public void SpawnWave(int waveIndex)
    {
        if (waveIndex < 0 || waveIndex >= waves.Count)
        {
            Debug.LogWarning($"WaveSpawner: 无效波次索引 {waveIndex}");
            return;
        }

        if (enemyRoot == null)
        {
            Debug.LogWarning("WaveSpawner: enemyRoot 未绑定。");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("WaveSpawner: spawnPoints 未绑定。");
            return;
        }

        WaveDefinition wave = waves[waveIndex];
        int totalSpawned = 0;

        Debug.Log($"SpawnWave -> 波次 {waveIndex + 1}");

        for (int i = 0; i < wave.entries.Count; i++)
        {
            WaveEntry entry = wave.entries[i];
            if (entry.enemyPrefab == null) continue;

            Debug.Log($"  配置敌人: {entry.enemyPrefab.name}, 数量: {entry.count}");

            for (int c = 0; c < entry.count; c++)
            {
                Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Instantiate(entry.enemyPrefab, point.position, Quaternion.identity, enemyRoot);
                totalSpawned++;
            }
        }

        Debug.Log($"  本次实际生成: {totalSpawned}");
    }

    public void ClearAllEnemies()
    {
        if (enemyRoot == null) return;

        for (int i = enemyRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(enemyRoot.GetChild(i).gameObject);
        }
    }
}