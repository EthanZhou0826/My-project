using UnityEngine;

public class PlayerCombatStats : MonoBehaviour
{
    [Header("References")]
    public PlayerShooter shooter;
    public PlayerHealth health;
    public Projectile projectilePrefab;

    [Header("Runtime Stats")]
    public float damageMultiplier = 1f;
    public float fireRateMultiplier = 1f;
    public float projectileSpeedMultiplier = 1f;
    public int bonusMaxHP = 0;
    public int bonusPierce = 0;

    private void Awake()
    {
        if (shooter == null) shooter = GetComponent<PlayerShooter>();
        if (health == null) health = GetComponent<PlayerHealth>();
    }

    public void AddDamagePercent(float value)
    {
        damageMultiplier += value;
        ApplyToProjectilePrefab();
    }

    public void AddFireRatePercent(float value)
    {
        fireRateMultiplier += value;

        if (shooter != null)
        {
            shooter.fireInterval /= (1f + value);
            shooter.fireInterval = Mathf.Max(0.05f, shooter.fireInterval);
        }
    }

    public void AddProjectileSpeedPercent(float value)
    {
        projectileSpeedMultiplier += value;
        ApplyToProjectilePrefab();
    }

    public void AddMaxHP(int value)
    {
        bonusMaxHP += value;

        if (health != null)
        {
            health.maxHP += value;
            health.ResetHP();
        }
    }

    public void Heal(int value)
    {
        if (health == null) return;

        int target = Mathf.Min(health.currentHP + value, health.maxHP);
        health.SetCurrentHP(target);
    }

    public void AddPierce(int value)
    {
        bonusPierce += value;
        ApplyToProjectilePrefab();
    }

    private void ApplyToProjectilePrefab()
    {
        if (projectilePrefab == null) return;

        projectilePrefab.damage = Mathf.Max(1, Mathf.RoundToInt(projectilePrefab.baseDamage * damageMultiplier));
        projectilePrefab.speed = projectilePrefab.baseSpeed * projectileSpeedMultiplier;
        projectilePrefab.pierceCount = projectilePrefab.basePierceCount + bonusPierce;
    }
}