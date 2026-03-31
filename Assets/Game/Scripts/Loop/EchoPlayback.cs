using UnityEngine;

public class EchoPlayback : MonoBehaviour
{
    public Transform visualRoot;
    public Transform firePoint;
    public Projectile projectilePrefab;
    public Transform projectileRoot;

    private EchoRecording recording;
    private float elapsedTime = 0f;
    private int frameIndex = 0;
    private int shotIndex = 0;
    private bool isPlaying = false;

    public void Initialize(EchoRecording newRecording, Transform newProjectileRoot)
    {
        recording = newRecording;
        projectileRoot = newProjectileRoot;
    }

    public void RestartPlayback()
    {
        if (recording == null || recording.frames.Count == 0)
        {
            isPlaying = false;
            return;
        }

        elapsedTime = 0f;
        frameIndex = 0;
        shotIndex = 0;
        isPlaying = true;

        ApplyFrame(recording.frames[0]);
    }

    private void Update()
    {
        if (!isPlaying || recording == null) return;

        elapsedTime += Time.deltaTime;

        while (frameIndex < recording.frames.Count - 1 &&
               recording.frames[frameIndex + 1].time <= elapsedTime)
        {
            frameIndex++;
        }

        ApplyFrame(recording.frames[frameIndex]);

        while (shotIndex < recording.shotTimes.Count &&
               recording.shotTimes[shotIndex] <= elapsedTime)
        {
            Fire();
            shotIndex++;
        }

        if (elapsedTime >= recording.frames[recording.frames.Count - 1].time)
        {
            isPlaying = false;
        }
    }

    private void ApplyFrame(EchoFrame frame)
    {
        transform.position = frame.position;

        if (visualRoot != null)
        {
            visualRoot.rotation = Quaternion.Euler(0f, 0f, frame.visualRotationZ);
        }
    }

    private void Fire()
    {
        if (firePoint == null || projectilePrefab == null || projectileRoot == null) return;

        Projectile projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation,
            projectileRoot
        );

        projectile.Initialize(firePoint.right);
    }
}