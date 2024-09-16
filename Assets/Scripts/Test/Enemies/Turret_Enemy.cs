using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Enemy : MonoBehaviour, Enemy
{
    public GameObject projectileEnemy; 
    public float attackInterval = 3f;  
    public GameObject playerObject;    

    public int health = 10; 

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
            yield return new WaitForSeconds(attackInterval); 
            Attack();  
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
