using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Test : MonoBehaviour
{
    public float speed = 5f;
    public float maxDistance;
    private Vector2 direction;
    private Vector2 startPosition;
    private Rigidbody2D rb;
    public GameObject Player_;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }
    public void SetRange(float r)
    {
        maxDistance = r;
    }
    void FixedUpdate()
    {
        rb.velocity = direction * speed;
        if (Vector2.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    public void GetDirection(Vector2 dir)
    {
        direction = dir;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.Damage(1);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
        
    }
}
