using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public WaveFlowController waveFlowController;

    [Header("Texts")]
    public TMP_Text hpText;
    public TMP_Text waveText;
    public TMP_Text enemyCountText;

    private void Update()
    {
        if (playerHealth != null && hpText != null)
        {
            hpText.text = $"HP: {playerHealth.currentHP} / {playerHealth.maxHP}";
        }

        if (waveFlowController != null)
        {
            if (waveText != null)
            {
                int currentWave = Mathf.Clamp(waveFlowController.currentWave, 1, waveFlowController.totalWaveCount);
                waveText.text = $"Wave {currentWave} / {waveFlowController.totalWaveCount}";
            }

            if (enemyCountText != null)
            {
                enemyCountText.text = $"Enemies: {waveFlowController.GetAliveEnemyCountPublic()}";
            }
        }
    }
}