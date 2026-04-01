using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyHealth : MonoBehaviour, ILoopResettable
{
    public int maxHP = 3;
    public int currentHP { get; private set; }
    public bool IsDead => currentHP <= 0;

    private Collider2D col;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private EnemyChaser chaser;

    private Color originalColor;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        chaser = GetComponent<EnemyChaser>();

        if (sr != null)
        {
            originalColor = sr.color;
        }

        ResetForLoop();
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        Debug.Log($"{name} HP = {currentHP}");

        if (currentHP == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (chaser != null)
        {
            chaser.enabled = false;
        }

        if (col != null)
        {
            col.enabled = false;
        }

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
        }

        if (sr != null)
        {
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.25f);
        }
    }

    public void ResetForLoop()
    {
        currentHP = maxHP;

        if (chaser != null)
        {
            chaser.enabled = true;
        }

        if (col != null)
        {
            col.enabled = true;
        }

        if (rb != null)
        {
            rb.simulated = true;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (sr != null)
        {
            sr.color = originalColor == default ? Color.white : originalColor;
        }
    }
}