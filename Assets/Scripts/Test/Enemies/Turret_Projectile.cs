using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Projectile : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 direction;
    public GameObject Player_;
    private Rigidbody2D rb;
    public LayerMask EnemyLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }
    public void SetDirection(Vector2 direc)
    {
        direction = direc;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == EnemyLayer)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Damage(1); 
            }
            Destroy(gameObject);
        }
    }
}
