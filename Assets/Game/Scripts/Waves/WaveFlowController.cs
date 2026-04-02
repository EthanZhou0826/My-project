using UnityEngine;
using TMPro;

public class WaveFlowController : MonoBehaviour
{
    public enum FlowState
    {
        Idle,
        Fighting,
        ClearedDelay,
        RewardChoosing,
        Finished,
        GameOver
    }

    [Header("References")]
    public WaveSpawner waveSpawner;
    public Transform enemyRoot;
    public Transform projectileRoot;
    public Transform playerSpawn;
    public PlayerHealth playerHealth;
    public PlayerCombatStats playerCombatStats;
    public RewardSelectPanel rewardPanel;
    public GameOverPanel gameOverPanel;

    [Header("HUD")]
    public TMP_Text waveText;
    public TMP_Text enemyCountText;

    [Header("Flow")]
    public float clearDelay = 2f;
    public int totalWaveCount = 5;

    [Header("Debug")]
    public int currentWave = 0;
    public FlowState state = FlowState.Idle;

    private float clearTimer = 0f;

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDead += HandlePlayerDead;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDead -= HandlePlayerDead;
        }
    }

    private void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.Hide();
        }

        if (rewardPanel != null)
        {
            rewardPanel.Hide();
        }

        StartNextWave();
    }

    private void Update()
    {
        UpdateHUD();

        if (state == FlowState.GameOver || state == FlowState.Finished)
        {
            return;
        }

        if (state == FlowState.Fighting)
        {
            if (GetAliveEnemyCount() == 0)
            {
                state = FlowState.ClearedDelay;
                clearTimer = clearDelay;
                Debug.Log($"Wave {currentWave} Cleared");
            }
        }
        else if (state == FlowState.ClearedDelay)
        {
            clearTimer -= Time.deltaTime;
            if (clearTimer <= 0f)
            {
                if (currentWave >= totalWaveCount)
                {
                    state = FlowState.Finished;
                    Debug.Log("Run Clear");
                }
                else
                {
                    OpenRewardPanel();
                }
            }
        }
    }

    private void StartNextWave()
    {
        currentWave++;

        ClearAllProjectiles();
        ResetPlayerToSpawn();

        if (waveSpawner != null)
        {
            waveSpawner.SpawnWave(currentWave - 1);
        }

        state = FlowState.Fighting;
        Debug.Log($"Wave Start: {currentWave}");
    }

    private void OpenRewardPanel()
    {
        if (rewardPanel == null)
        {
            Debug.LogError("WaveFlowController: rewardPanel 没有绑定。");
            return;
        }

        state = FlowState.RewardChoosing;

        RewardSelectPanel.RewardOption[] options = BuildRewardOptions();
        rewardPanel.Show(options, ApplyRewardAndContinue);
    }

    private RewardSelectPanel.RewardOption[] BuildRewardOptions()
    {
        return new RewardSelectPanel.RewardOption[]
        {
            new RewardSelectPanel.RewardOption
            {
                type = RewardSelectPanel.RewardType.DamageUp,
                title = "猛攻",
                description = "伤害 +20%"
            },
            new RewardSelectPanel.RewardOption
            {
                type = RewardSelectPanel.RewardType.FireRateUp,
                title = "疾射",
                description = "射速 +15%"
            },
            new RewardSelectPanel.RewardOption
            {
                type = RewardSelectPanel.RewardType.MaxHpUp,
                title = "强体",
                description = "最大生命 +2"
            }
        };
    }

    private void ApplyRewardAndContinue(RewardSelectPanel.RewardOption option)
    {
        if (playerCombatStats == null)
        {
            Debug.LogError("WaveFlowController: playerCombatStats 没有绑定。");
            return;
        }

        Debug.Log($"选择强化: {option.title} / {option.description}");

        switch (option.type)
        {
            case RewardSelectPanel.RewardType.DamageUp:
                playerCombatStats.AddDamagePercent(0.20f);
                break;

            case RewardSelectPanel.RewardType.FireRateUp:
                playerCombatStats.AddFireRatePercent(0.15f);
                break;

            case RewardSelectPanel.RewardType.MaxHpUp:
                playerCombatStats.AddMaxHP(2);
                break;
        }

        if (waveSpawner != null)
        {
            waveSpawner.ClearAllEnemies();
        }

        StartNextWave();
    }

    private void HandlePlayerDead()
    {
        if (state == FlowState.GameOver) return;

        state = FlowState.GameOver;

        ClearAllProjectiles();

        if (waveSpawner != null)
        {
            waveSpawner.ClearAllEnemies();
        }

        Debug.Log("Game Over");

        if (gameOverPanel != null)
        {
            gameOverPanel.Show();
        }
    }

    private void ResetPlayerToSpawn()
    {
        if (playerSpawn == null || playerHealth == null) return;

        PlayerMotor motor = playerHealth.GetComponent<PlayerMotor>();
        if (motor != null)
        {
            motor.ResetToPosition(playerSpawn.position);
        }
        else
        {
            playerHealth.transform.position = playerSpawn.position;
        }
    }

    private void ClearAllProjectiles()
    {
        if (projectileRoot == null) return;

        for (int i = projectileRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(projectileRoot.GetChild(i).gameObject);
        }
    }

    private int GetAliveEnemyCount()
    {
        if (enemyRoot == null) return 0;

        int count = 0;
        EnemyHealth[] enemies = enemyRoot.GetComponentsInChildren<EnemyHealth>(true);

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null && !enemies[i].IsDead)
            {
                count++;
            }
        }

        return count;
    }

    public int GetAliveEnemyCountPublic()
    {
        return GetAliveEnemyCount();
    }

    private void UpdateHUD()
    {
        if (waveText != null)
        {
            int displayWave = Mathf.Clamp(currentWave, 1, totalWaveCount);
            waveText.text = $"Wave {displayWave} / {totalWaveCount}";
        }

        if (enemyCountText != null)
        {
            enemyCountText.text = $"Enemies: {GetAliveEnemyCount()}";
        }
    }
}