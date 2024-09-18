using System.Collections.Generic;
using UnityEngine;

public class Map_Controller : MonoBehaviour
{
    public LayerMask doorLayer; 
    public float detectionRadius = 1f; 

    public void OnPlayerEnterRoom(Room currentRoom)
    {
        CheckAdjacentRooms(currentRoom);
    }

    private void CheckAdjacentRooms(Room currentRoom)
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapBoxAll(currentRoom.transform.position, new Vector2(detectionRadius, detectionRadius), 0f, doorLayer);

        foreach (Collider2D collider in nearbyColliders)
        {
            Room adjacentRoom = collider.GetComponent<Room>();
            if (adjacentRoom != null && !adjacentRoom.IsVisited)
            {
                Vector2 direction = GetDirectionFromCollider(collider);
                adjacentRoom.UpdateRoomIcons(direction); 
                adjacentRoom.SetAccessible(); 
            }
        }
    }

    private Vector2 GetDirectionFromCollider(Collider2D collider)
    {
        Vector2 direction = Vector2.zero;
        Vector2 offset = collider.transform.position - transform.position;
        if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
        {
            direction = offset.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            direction = offset.y > 0 ? Vector2.up : Vector2.down;
        }
        return direction;
    }
}
