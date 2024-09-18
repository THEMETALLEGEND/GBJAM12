using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Enemy : MonoBehaviour, Enemy
{
    public GameObject projectileEnemy; 
    public float attackInterval = 3f;  
    public GameObject playerObject;
    private int room;
    public int health = 10;

    void Start()
    {
        StartCoroutine(ShootingProjectile());
    }

    public void Damage(int damageAmount)
    {
        if (playerObject.GetComponent<PlayerMovement>().actual_Room == room) // prevents the enemy from taking damage if the player is not on the same room.
        {
            health -= damageAmount;
            Debug.Log("damage taken: " + damageAmount);
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
        
    }
    public void SetRoom(int r)
    {
        room = r; //gets the actual room from the room script.
    }
    IEnumerator ShootingProjectile()
    {

        while (true)
        {
            if (room == playerObject.GetComponent<PlayerMovement>().actual_Room) // checks if the player is on the same room to attack it.
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
        if (projectileEnemy != null && playerObject != null)  
        {
            Vector2 playerPosition = playerObject.transform.position;
            Vector2 attackDirection = (playerPosition - (Vector2)transform.position).normalized;

            GameObject attack = Instantiate(projectileEnemy, transform.position, transform.rotation);
            attack.SetActive(true);

            Turret_Projectile projectileScript = attack.GetComponent<Turret_Projectile>();
            projectileScript.SetDirection(attackDirection);

        }
    }
}
