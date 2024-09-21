using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors_Controller : MonoBehaviour
{
    public LayerMask enemyLayer;
    public LayerMask doorLayer;
    private Collider2D roomCollider;
    public AudioClip openSound;
    public AudioClip closeSound;
    private AudioSource source;
    public bool isAlreadyOpen = false;
    private bool isOnBattle = false;

    private void Start()
    {
        //gets the collider of the room to check if there are enemies
        roomCollider = GetComponent<Collider2D>();
        source = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (isOnBattle)
        {
            Collider2D[] EnemiesinRoom = Physics2D.OverlapBoxAll(roomCollider.bounds.center, roomCollider.bounds.size, 0f, LayerMask.GetMask("Enemy"));
            if (EnemiesinRoom.Length <= 0)
            {
                HandleDoors(true);
            }
        }
        /*/checks if there are enemies in the room with the collider properties
        Collider2D[] enemies = Physics2D.OverlapBoxAll(roomCollider.bounds.center, roomCollider.bounds.size, 0f, enemyLayer);
        // if there are no enemies it deactivate the doors
        if (enemies.Length <= 0)
        {
            HandleDoors(true);
        }
        */
    }

    /*public void CloseDoors() //called by the function of the room trigger, to happen when the player enters the room
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(roomCollider.bounds.center, roomCollider.bounds.size, 0f, enemyLayer);
        if (enemies.Length > 0)
        {
            HandleDoors(false);
        }
    }*/
    public void OnRoomEnter()
    {
        Collider2D[] EnemiesinRoom = Physics2D.OverlapBoxAll(roomCollider.bounds.center, roomCollider.bounds.size, 0f, LayerMask.GetMask("Enemy"));
        if (EnemiesinRoom.Length <= 0)
        {
            HandleDoors(true);
        }
        else
        {
            HandleDoors(false);
            gameObject.GetComponent<Room>().FindAndActivateSpawners();
        }
    }
    public void HandleDoors(bool open)
    {
        Collider2D[] doors = Physics2D.OverlapBoxAll(roomCollider.bounds.center, roomCollider.bounds.size, 0f, doorLayer);

        foreach (var door in doors)
        {
            Debug.Log($"Detected door: {door.name}");
            //try to find the open door and closed door objects to handle them
            Transform openDoor = door.transform.GetComponentInChildren<Transform>().Find("OpenDoor");
            Transform closedDoor = door.transform.GetComponentInChildren<Transform>().Find("ClosedDoor");

            if (openDoor != null && closedDoor != null)
            {
                if (open) //if it needs to be open
                {
                    openDoor.gameObject.SetActive(true);
                    closedDoor.gameObject.SetActive(false);
                    if (!isAlreadyOpen)
                    {
                        source.clip = openSound;
                        source.Play();
                        isAlreadyOpen = true;  
                    }
                    
                }
                else if (!isAlreadyOpen) // if it needs to be closed
                {
                    openDoor.gameObject.SetActive(false);
                    closedDoor.gameObject.SetActive(true);
                    source.clip = closeSound;
                    source.Play();
                    isOnBattle = true; // triggers so the update can now check for enemies (checking before caused bugs)
                }
            }
        }
    }
}
