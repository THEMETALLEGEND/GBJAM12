using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors_script : MonoBehaviour
{
    public LayerMask doors_layer;
    public LayerMask enemy_layer;
    private Vector2 boxSize;

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

        Collider2D[] enemyColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(10,10), 0f, enemy_layer);
        if (enemyColliders.Length < 1)
        {
            Collider2D[] doorColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(10, 10), 0f, doors_layer);
            foreach (Collider2D door in doorColliders)
            {
                door.gameObject.SetActive(false);
                Debug.Log("Doors deactivated");
            }
        }
    }
}
