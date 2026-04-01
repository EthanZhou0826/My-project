using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 16f;
    public float lifeTime = 0.9f;
    public int damage = 1;

    private Vector2 direction = Vector2.right;
    private float timer = 0f;

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
        timer = 0f;
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
            Destroy(gameObject);
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }
}