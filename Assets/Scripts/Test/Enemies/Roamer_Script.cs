using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roamer_Script : MonoBehaviour, Enemy
{
    private bool canMove = true;
    private int health = 2;
    private int room;
    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private bool choosed_direction;
    private bool isInKnockBack;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MovementRoutine());
    }

    private void FixedUpdate()
    {
        if (isInKnockBack)
        {
            return; // If in knockback, ignore normal movement
        }

        if (canMove)
        {
            rb.velocity = moveDirection;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
            Destroy(gameObject);
    }

    public void SetRoom(int r)
    {
        room = r;
    }

    private IEnumerator MovementRoutine()
    {
        while (true)
        {
            if (!isInKnockBack)
            {
                moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                canMove = true;
                yield return new WaitForSeconds(2f);

                canMove = false;
                rb.velocity = Vector2.zero;
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return null;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            coll.gameObject.GetComponent<PlayerAttacking>().TakeDamage(1, transform.position);
        }
    }

    public void KnockBack_(Vector2 knockbackDirection)
    {
        Vector2 direction = (transform.position - (Vector3)knockbackDirection).normalized;
        StartCoroutine(KnockBackRoutine(direction));
    }

    private IEnumerator KnockBackRoutine(Vector2 knockbackDirection)
    {
        isInKnockBack = true;
        canMove = false;
        rb.velocity = knockbackDirection * 3f;

        yield return new WaitForSeconds(1f);

        canMove = true;
        rb.velocity = Vector2.zero;
        isInKnockBack = false;
    }
}
