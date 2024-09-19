using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool IsVisited = false;
    public GameObject RoomIcon;
    public Map_Controller mapController;
    private SpriteRenderer roomIconRenderer;
    public LayerMask playerLayer;
    public int roomNumber;
    public LayerMask enemyLayer;

    //gets the transform of the room to use as reference to the map
    public Vector2 Position
    {
        get { return new Vector2(transform.position.x, transform.position.y); }
    }

    void Start()
    {
        //sets the icons as black so it becomes invisible. didnt disable because it could cause more problems.
        roomIconRenderer = RoomIcon.GetComponent<SpriteRenderer>();
        roomIconRenderer.color = Color.black;
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(transform.position, new Vector2(9, 9), 0f, enemyLayer);
        foreach (Collider2D collider in hitEnemies)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SetRoom(roomNumber);// sets the rooms for each enemy that is on the interface Enemy.cs.
            }
        }
    }
    /*void OnTriggerEnter2D(Collider2D coll)
    {
        //if player collides with the room it sets it as visited and white on the map.
        if (coll.gameObject.CompareTag("Player"))
        {
            Doors_Controller doors = gameObject.GetComponent<Doors_Controller>();
            doors.CloseDoors();
            coll.GetComponent<PlayerMovement>().actual_Room = roomNumber;
            IsVisited = true;
            roomIconRenderer.color = Color.white;
            mapController.OnPlayerEnterRoom(this);
        }
    }*/
    //if this room is acessible by doors but not entered before it becomes green
    public void SetAccessible()
    {
        roomIconRenderer.color = Color.yellow;
    }

    public void UpdateRoomIcons(Vector2 direction) // used for the map system
    {
        RoomIcon.SetActive(true);
        if (direction == Vector2.right)
        {
            RoomIcon.transform.position = new Vector2(transform.position.x + 1, transform.position.y);
        }
        else if (direction == Vector2.left)
        {
            RoomIcon.transform.position = new Vector2(transform.position.x - 1, transform.position.y);
        }
        else if (direction == Vector2.up)
        {
            RoomIcon.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
        }
        else if (direction == Vector2.down)
        {
            RoomIcon.transform.position = new Vector2(transform.position.x, transform.position.y - 1);
        }
    }
}
