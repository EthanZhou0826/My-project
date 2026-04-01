using System.Collections.Generic;
using UnityEngine;

public class RoomClearController : MonoBehaviour
{
    [Header("Targets")]
    public Transform enemyRoot;

    [Header("Debug")]
    public bool roomCleared;

    private readonly List<EnemyHealth> cachedEnemies = new List<EnemyHealth>();

    private void Start()
    {
        RefreshEnemyList();
    }

    private void Update()
    {
        if (roomCleared) return;

        if (enemyRoot == null)
        {
            Debug.LogWarning("RoomClearController: enemyRoot Ã»ÓÐ°ó¶¨¡£");
            return;
        }

        bool allDead = true;
        int aliveCount = 0;

        for (int i = 0; i < cachedEnemies.Count; i++)
        {
            EnemyHealth enemy = cachedEnemies[i];
            if (enemy == null) continue;

            if (!enemy.IsDead)
            {
                allDead = false;
                aliveCount++;
            }
        }

        if (allDead)
        {
            roomCleared = true;
            Debug.Log("Room Clear");
        }
    }

    public void RefreshEnemyList()
    {
        cachedEnemies.Clear();

        if (enemyRoot == null) return;

        EnemyHealth[] enemies = enemyRoot.GetComponentsInChildren<EnemyHealth>(true);
        for (int i = 0; i < enemies.Length; i++)
        {
            cachedEnemies.Add(enemies[i]);
        }
    }

    public int GetAliveEnemyCount()
    {
        int count = 0;

        for (int i = 0; i < cachedEnemies.Count; i++)
        {
            EnemyHealth enemy = cachedEnemies[i];
            if (enemy != null && !enemy.IsDead)
            {
                count++;
            }
        }

        return count;
    }
}