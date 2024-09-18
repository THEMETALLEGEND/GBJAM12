using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Projectile : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 direction;
    private Rigidbody2D rb;
    public LayerMask PlayerLayer;

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
    //when colliding with player calls the function that applies damage
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerAttacking player = other.GetComponent<PlayerAttacking>();
            if (player != null)
            {
                player.TakeDamage(1); 
            }
            Destroy(gameObject);
        }
    }
}
