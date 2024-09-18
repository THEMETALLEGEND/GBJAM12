using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Test : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction;
    private Rigidbody2D rb;
    public PlayerAttacking direction_script;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = direction_script.magicDirection;
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
            Turret_Enemy enemy = other.GetComponent<Turret_Enemy>();

            if (enemy != null)
            {
                enemy.Damage(1);
            }
            else
            {
                Debug.LogError("Player script not found");
            }
            Destroy(gameObject);
        }
    }
}
