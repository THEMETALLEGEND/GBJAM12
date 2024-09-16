using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Test : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 direction;
    private Rigidbody2D rb;
    public PlayerMovement direction_script;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //direction = direction_script.Direction_Selected;
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }
    public void GetDirection(Vector2 dir)
    {
        direction = dir;
    }
}
