using UnityEngine;

public class LoopRecorder : MonoBehaviour
{
    public float sampleInterval = 0.02f;
    public PlayerAimer aimer;

    private EchoRecording currentRecording;
    private float elapsedTime = 0f;
    private float sampleTimer = 0f;
    private bool isRecording = false;

    private void Awake()
    {
        if (aimer == null)
        {
            aimer = GetComponent<PlayerAimer>();
        }
    }

    public void BeginRecording()
    {
        currentRecording = new EchoRecording();
        elapsedTime = 0f;
        sampleTimer = 0f;
        isRecording = true;

        RecordFrameNow();
    }

    public EchoRecording EndRecording()
    {
        isRecording = false;

        if (currentRecording == null)
            return new EchoRecording();

        return currentRecording.Clone();
    }

    public void RecordShotNow()
    {
        if (!isRecording || currentRecording == null) return;
        currentRecording.shotTimes.Add(elapsedTime);
    }

    private void Update()
    {
        if (!isRecording) return;

        elapsedTime += Time.deltaTime;
        sampleTimer += Time.deltaTime;

        if (sampleTimer >= sampleInterval)
        {
            sampleTimer = 0f;
            RecordFrameNow();
        }
    }

    private void RecordFrameNow()
    {
        if (currentRecording == null) return;

        EchoFrame frame = new EchoFrame();
        frame.time = elapsedTime;
        frame.position = transform.position;
        frame.visualRotationZ = aimer != null ? aimer.GetVisualRotationZ() : 0f;

        currentRecording.frames.Add(frame);
    }
}