using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Enemy : MonoBehaviour, Enemy
{
    private int health = 10;
    public void Damage(int damageAmount)
    {
        Debug.Log("damage applied");
        health -= damageAmount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
