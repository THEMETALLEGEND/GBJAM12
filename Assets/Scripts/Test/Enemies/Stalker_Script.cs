using Pathfinding;
using System.Collections;
using UnityEngine;

public class Stalker_Script : MonoBehaviour, Enemy
{
    public Transform target;
    private Rigidbody2D rb;
    private bool isPaused = false;
    private int health = 2;
    public int room { get; set; }
    public Room_TransitionCollision roomsTransition; 

    public GameObject coin_prefab;
    private AudioSource audio;

    public IEnumerator DropCoins(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject Coin = Instantiate(coin_prefab, transform.position, transform.rotation);
        }
        AIPath ai = GetComponent<AIPath>();
        ai.canMove = false;
        yield return new WaitForSeconds(1); 
        Destroy(gameObject);
    }

    void Start()
    {
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        FindRoomTransition(); 
        StartCoroutine(transitionTime()); 
    }

    private void FindRoomTransition()
    {
        Room_TransitionCollision transitionCollider = GetComponentInParent<Room_TransitionCollision>();
        if (transitionCollider != null)
        {
            roomsTransition = transitionCollider; 
        }
    }

    private IEnumerator transitionTime()
    {
        AIPath ai = GetComponent<AIPath>();
        ai.canMove = false;
        yield return new WaitForSeconds(1f);
        ai.canMove = true;
    }

    public void SetRoom(int r)
    {
        room = r;
    }

    public void Damage(int damageAmount)
    {
        audio.Play();
        if (roomsTransition.actual_Room == room)
        {
            health -= damageAmount;
            if (health <= 0)
            {
                StartCoroutine(DropCoins(1));
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
        AIPath ai = GetComponent<AIPath>();
        ai.canMove = false;

        rb.velocity = knockbackDirection * 2f;

        yield return new WaitForSeconds(1f);

        rb.velocity = Vector2.zero;
        ai.canMove = true;
    }
}
