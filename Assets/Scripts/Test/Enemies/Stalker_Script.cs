using Pathfinding; 
using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour, Enemy
{
    public Transform target; 

    private Rigidbody2D rb;
    private bool isPaused = false;
    private int health = 2;
    private int room;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    public void SetRoom(int r)
    {
        room = r;
    }
    void FixedUpdate()
    {
    }
    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
            Destroy(gameObject);
    }
    private IEnumerator PauseMovement(float duration)
    {
        isPaused = true;
        rb.velocity = Vector2.zero; 
        yield return new WaitForSeconds(duration);
        isPaused = false;
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
        AIPath ai = GetComponent<AIPath>();
        ai.canMove = false; 

        rb.velocity = knockbackDirection * 3f;

        yield return new WaitForSeconds(1f); 

        rb.velocity = Vector2.zero;
        ai.canMove = true;
    }
}
