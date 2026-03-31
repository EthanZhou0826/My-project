using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Transform firePoint;
    public Projectile projectilePrefab;
    public Transform projectileRoot;
    public float fireInterval = 0.25f;

    private float nextFireTime = 0f;
    private LoopRecorder recorder;

    private void Awake()
    {
        recorder = GetComponent<LoopRecorder>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireInterval;
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

        if (recorder != null)
        {
            recorder.RecordShotNow();
        }
    }
}