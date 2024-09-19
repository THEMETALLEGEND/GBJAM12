using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_CollisionScript : MonoBehaviour
{
    [HideInInspector] public bool is_Attacking;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Enemy") && is_Attacking == true)
        {
            coll.gameObject.GetComponent<Enemy>().Damage(1);
            coll.gameObject.GetComponent<Enemy>().KnockBack_(transform.position);
        }
    }
}
