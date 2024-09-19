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
            actual_Room = other.gameObject.GetComponent<Room>().roomNumber;
        }
    }
}
