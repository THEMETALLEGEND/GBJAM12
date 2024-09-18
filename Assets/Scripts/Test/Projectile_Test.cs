using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Test : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction;
    private Rigidbody2D rb;
    public GameObject Player_;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed;
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
                //enemy.KnockBack_(Player_.transform.position);
            }
            else
            {
                Debug.LogError("Enemy script not found");
            }
            Destroy(gameObject);
        }
    }
}
