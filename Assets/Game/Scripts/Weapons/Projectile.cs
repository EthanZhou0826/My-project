using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Base Stats")]
    public int baseDamage = 1;
    public float baseSpeed = 18f;
    public int basePierceCount = 0;

    [Header("Runtime Stats")]
    public int damage = 1;
    public float speed = 18f;
    public int pierceCount = 0;

    public float lifeTime = 0.8f;

    private Vector2 direction = Vector2.right;
    private float timer;
    private int hitCount;

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
        timer = 0f;
        hitCount = 0;
    }

    private void Awake()
    {
        damage = baseDamage;
        speed = baseSpeed;
        pierceCount = basePierceCount;
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerHealth>() != null) return;

        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            hitCount++;

            if (hitCount > pierceCount)
            {
                Destroy(gameObject);
            }
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }
}