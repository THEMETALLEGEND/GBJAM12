using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Enemy : MonoBehaviour, Enemy
{
    public GameObject projectileEnemy; 
    public float attackInterval = 3f;  
    public GameObject transitionObject;
    [HideInInspector] public int room { get; set; }
    public int health = 10;
    public GameObject coin_prefab;
    private AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        StartCoroutine(ShootingProjectile());
    }

    public void Damage(int damageAmount)
    {
        audio.Play();
        if (transitionObject.GetComponent<Room_TransitionCollision>().actual_Room == room) // prevents the enemy from taking damage if the player is not on the same room.
        {
            health -= damageAmount;
            Debug.Log("damage taken: " + damageAmount);
            if (health <= 0)
            {
                StartCoroutine(DropCoins(1));
            }
        }
    }

    public IEnumerator DropCoins(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject Coin = Instantiate(coin_prefab, transform.position, transform.rotation);
        }
        yield return new WaitForSeconds(1); // this is where we can put the animation of death of the enemies
        Destroy(gameObject);
    }

    public void SetRoom(int r)
    {
        room = r; //gets the actual room from the room script.
    }

    IEnumerator ShootingProjectile()
    {

        while (true)
        {
            if (transitionObject.GetComponent<Room_TransitionCollision>().actual_Room == room && health > 0) // checks if the player is on the same room to attack it.
            {
                yield return new WaitForSeconds(attackInterval);
                Attack();
            }
            else
            {
                yield return null;  
            }
        }
    }

    void Attack()
    {
        if (projectileEnemy != null && transitionObject != null)  
        {
            Vector2 playerPosition = transitionObject.transform.position;
            Vector2 attackDirection = (playerPosition - (Vector2)transform.position).normalized;

            GameObject attack = Instantiate(projectileEnemy, transform.position, transform.rotation);
            attack.SetActive(true);

            Turret_Projectile projectileScript = attack.GetComponent<Turret_Projectile>();
            projectileScript.SetDirection(attackDirection);

        }
    }
    public void KnockBack_(Vector2 knockbackDirection)
    {
        StartCoroutine(KnockBackRoutine(knockbackDirection));
    }
    // if needed we can change that so it doesnt have knock back
    private IEnumerator KnockBackRoutine(Vector2 knockbackDirection)
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = knockbackDirection * 1;
        yield return new WaitForSeconds(1);
        rb.velocity = Vector2.zero;
    }

}
