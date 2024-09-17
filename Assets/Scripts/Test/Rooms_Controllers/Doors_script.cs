using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors_script : MonoBehaviour
{
    public LayerMask doors_layer;  
    public LayerMask enemy_layer;  
    public int roomnumber;        
    private Vector2 boxSize = new Vector2(10, 10); 

    private Room_Controller roomsManager;  

    void Start()
    {
        roomsManager = FindObjectOfType<Room_Controller>(); 
        UpdateDoorState();  
    }

    void Update()
    {
        UpdateDoorState();  
    }

    void UpdateDoorState()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f, enemy_layer);
        bool isInCurrentRoom = roomsManager.room_entered == roomnumber; 

        if (isInCurrentRoom)
        {
            if (enemyColliders.Length > 0)
            {
                SetDoorState(false);  
            }
            else
            {
                SetDoorState(true);  
            }
        }
    }

    void SetDoorState(bool isOpen)
    {
        Collider2D[] doorColliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f, doors_layer);

        foreach (Collider2D door in doorColliders)
        {
            if (isOpen)
            {
                door.isTrigger = true;
            }
            else
            {
                door.isTrigger = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
