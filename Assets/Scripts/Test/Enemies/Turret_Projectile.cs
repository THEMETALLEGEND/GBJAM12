using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Projectile : MonoBehaviour
{
    public float speed = 5f;
    public float maxDistance = 10f;
    private Vector3 direction;
    private Vector3 startPosition;
    private Rigidbody2D rb;
    public LayerMask PlayerLayer;
    public GameObject Turret;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed;
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector2 direc)
    {
        direction = direc;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && other != null)
        {
            PlayerAttacking player = other.GetComponent<PlayerAttacking>();
            if (player != null)
            {
                player.TakeDamage(1, Turret.transform.position);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Obstacle") && other != null){
            Destroy(gameObject);
        }
    }
}
