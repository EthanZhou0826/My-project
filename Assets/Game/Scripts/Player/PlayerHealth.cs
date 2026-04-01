using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 5;
    public int currentHP { get; private set; }

    public bool IsDead => currentHP <= 0;

    public event Action OnDead;

    private bool deadEventSent;

    private void Awake()
    {
        ResetHP();
    }

    public void ResetHP()
    {
        currentHP = maxHP;
        deadEventSent = false;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        Debug.Log("Player HP = " + currentHP);

        if (currentHP == 0 && !deadEventSent)
        {
            deadEventSent = true;
            OnDead?.Invoke();
        }
    }
}