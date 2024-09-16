using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors_script : MonoBehaviour
{
    public LayerMask doors_layer;
    public LayerMask enemy_layer;
    public int roomnumber;

    void Start()
    {
        Collider2D[] doorColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(10, 10), 0f, doors_layer);
        foreach (Collider2D door in doorColliders)
        {
            door.gameObject.SetActive(true);
        }
    }
    void Update()
    {

        Collider2D[] enemyColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(10, 10), 0f, enemy_layer);
        if (enemyColliders.Length < 1)
        {
            Collider2D[] doorColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(10, 10), 0f, doors_layer);
            foreach (Collider2D door in doorColliders)
            {
                //later i will substitute this setactive for calling a function that changes the animation of the door to the open one
                door.gameObject.SetActive(false);
                Debug.Log("Doors deactivated");
            }
        }
    }
}
