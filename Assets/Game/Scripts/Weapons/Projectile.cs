using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 12f;
    public float lifeTime = 2f;

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
}