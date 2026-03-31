using System.Collections.Generic;
using UnityEngine;

public class LoopDirector : MonoBehaviour
{
    [Header("Loop Settings")]
    public float loopDuration = 10f;
    public int maxLoops = 3;

    [Header("Scene References")]
    public Transform playerSpawn;
    public PlayerMotor playerMotor;
    public PlayerAimer playerAimer;
    public PlayerHealth playerHealth;
    public LoopRecorder loopRecorder;

    public EchoPlayback echoPrefab;
    public Transform echoRoot;
    public Transform projectileRoot;

    private float timer;
    private int currentLoop = 1;
    private bool isFinished = false;

    private readonly List<EchoPlayback> echoes = new List<EchoPlayback>();

    private void Start()
    {
        timer = loopDuration;

        if (playerHealth != null)
            playerHealth.ResetHP();

        if (loopRecorder != null)
            loopRecorder.BeginRecording();
    }

    private void Update()
    {
        if (isFinished) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            HandleLoopEnd();
        }
    }

    private void HandleLoopEnd()
    {
        // 结束当前录制
        EchoRecording recording = null;
        if (loopRecorder != null)
        {
            recording = loopRecorder.EndRecording();
        }

        // 生成一个新分身
        if (recording != null && recording.frames.Count > 0 && echoPrefab != null && echoRoot != null)
        {
            EchoPlayback newEcho = Instantiate(echoPrefab, echoRoot);
            newEcho.Initialize(recording, projectileRoot);
            echoes.Add(newEcho);
        }

        // 如果已到最后一轮，结束
        if (currentLoop >= maxLoops)
        {
            isFinished = true;
            Debug.Log("Loop test finished.");
            return;
        }

        currentLoop++;
        timer = loopDuration;

        ResetPlayerToSpawn();
        ClearProjectiles();
        RestartAllEchoes();

        if (playerHealth != null)
            playerHealth.ResetHP();

        if (loopRecorder != null)
            loopRecorder.BeginRecording();

        Debug.Log("Loop Start: " + currentLoop);
    }

    private void ResetPlayerToSpawn()
    {
        if (playerMotor != null && playerSpawn != null)
        {
            playerMotor.ResetToPosition(playerSpawn.position);
        }

        if (playerAimer != null && playerSpawn != null)
        {
            playerAimer.SetVisualRotation(playerSpawn.eulerAngles.z);
        }
    }

    private void ClearProjectiles()
    {
        if (projectileRoot == null) return;

        for (int i = projectileRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(projectileRoot.GetChild(i).gameObject);
        }
    }

    private void RestartAllEchoes()
    {
        for (int i = 0; i < echoes.Count; i++)
        {
            if (echoes[i] != null)
            {
                echoes[i].RestartPlayback();
            }
        }
    }
}