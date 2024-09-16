using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Controller : MonoBehaviour
{
    public int room_entered;
    public BoxCollider2D coll;
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyDetecter"))
        {
            Debug.Log("new room detected");
            Doors_script script = collision.GetComponent<Doors_script>();
            room_entered = script.roomnumber;
        }
    }
}
