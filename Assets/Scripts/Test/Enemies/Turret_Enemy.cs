using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Enemy : MonoBehaviour, Enemy
{
    public GameObject projectileEnemy; 
    public float attackInterval = 3f;  
    public GameObject playerObject;
    public int room;
    public int health = 10;
    public int havetochangelater = 1; //has to be changed for the room index.

    void Start()
    {
        StartCoroutine(ShootingProjectile());
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        Debug.Log("damage taken: " + damageAmount);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ShootingProjectile()
    {

        while (true)
        {
            if (room == havetochangelater)
            {
                yield return new WaitForSeconds(attackInterval);
                Attack();
            }
            else
            {
                yield return null;  // Wait until the next frame to check the condition again
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
