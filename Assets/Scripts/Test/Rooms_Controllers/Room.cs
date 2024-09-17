using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool IsVisited = false;
    public GameObject RoomIcon;
    public Map_Controller mapController;
    public Color visitedColor = Color.white;
    public Color accessibleColor = Color.yellow;
    private SpriteRenderer roomIconRenderer;
    public LayerMask playerLayer;

    public Vector2 Position
    {
        get { return new Vector2(transform.position.x, transform.position.y); }
    }

    void Start()
    {
        roomIconRenderer = RoomIcon.GetComponent<SpriteRenderer>();
        roomIconRenderer.color = Color.black;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (playerLayer == (playerLayer | (1 << coll.gameObject.layer)))
        {
            IsVisited = true;
            RoomIcon.SetActive(true);
            roomIconRenderer.color = visitedColor;
            mapController.OnPlayerEnterRoom(this);
        }
    }

    public void SetAccessible()
    {
        roomIconRenderer.color = accessibleColor;
    }

    public void UpdateRoomIcons(Vector2 direction)
    {
        RoomIcon.SetActive(true);
        if (direction == Vector2.right)
        {
            RoomIcon.transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        }
        else if (direction == Vector2.left)
        {
            RoomIcon.transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        }
        else if (direction == Vector2.up)
        {
            RoomIcon.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }
        else if (direction == Vector2.down)
        {
            RoomIcon.transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        }
    }
}
