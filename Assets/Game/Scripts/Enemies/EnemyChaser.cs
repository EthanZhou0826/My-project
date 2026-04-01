using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyChaser : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 2.8f;
    public float stopDistance = 0.08f;

    [Header("Contact Damage")]
    public int touchDamage = 1;
    public float damageInterval = 0.75f;

    private Rigidbody2D rb;
    private Transform target;
    private float nextDamageTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        FindPlayerTarget();
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            FindPlayerTarget();
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 delta = (Vector2)target.position - rb.position;

        if (delta.sqrMagnitude <= stopDistance * stopDistance)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        rb.velocity = delta.normalized * moveSpeed;
    }

    private void FindPlayerTarget()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Time.time < nextDamageTime) return;

        PlayerHealth hp = collision.collider.GetComponent<PlayerHealth>();
        if (hp == null) return;

        hp.TakeDamage(touchDamage);
        nextDamageTime = Time.time + damageInterval;
    }
}