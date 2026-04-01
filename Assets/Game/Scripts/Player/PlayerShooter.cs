using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public Projectile projectilePrefab;
    public Transform projectileRoot;

    [Header("Fire")]
    public float fireInterval = 0.20f;
    public bool holdToFire = true;

    private float nextFireTime;

    private void Update()
    {
        bool wantFire = holdToFire ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);

        if (!wantFire) return;
        if (Time.time < nextFireTime) return;

        Fire();
        nextFireTime = Time.time + fireInterval;
    }

    private void Fire()
    {
        if (firePoint == null)
        {
            Debug.LogWarning("PlayerShooter: firePoint Î´°ó¶¨¡£");
            return;
        }

        if (projectilePrefab == null)
        {
            Debug.LogWarning("PlayerShooter: projectilePrefab Î´°ó¶¨¡£");
            return;
        }

        Projectile projectile;

        if (projectileRoot != null)
        {
            projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation, projectileRoot);
        }
        else
        {
            projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }

        projectile.Initialize(firePoint.right);
    }
}