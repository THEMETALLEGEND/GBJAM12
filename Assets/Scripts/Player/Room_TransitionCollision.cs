using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_TransitionCollision : MonoBehaviour
{
    public int actual_Room = 1;
    public LayerMask roomsMask;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(((1 << other.gameObject.layer) & roomsMask) != 0)
        {
            Room roomScript = other.gameObject.GetComponent<Room>();
            actual_Room = roomScript.roomNumber;
            roomScript.GenerateEnemies();
            other.GetComponent<Doors_Controller>().CloseDoors();
        }
    }
}
