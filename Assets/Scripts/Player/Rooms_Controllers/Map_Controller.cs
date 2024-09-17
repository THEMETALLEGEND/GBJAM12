using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Controller : MonoBehaviour
{
    public GameObject[] roomMapIcons; 

    private bool[] visitedRooms;
    public int currentRoom; 

    void Start()
    {
        visitedRooms = new bool[roomMapIcons.Length];
        UpdateMap();
    }

    public void EnterRoom(int roomIndex)
    {
        currentRoom = roomIndex;

        visitedRooms[roomIndex] = true;

        UpdateMap();
    }

    void UpdateMap()
    {

        switch (currentRoom)
        {
            case 0: 
                ShowRoom(0); 
                ShowAdjacentRoom(1); 
                ShowAdjacentRoom(2); 
                break;

            case 1: 
                ShowRoom(1); 
                ShowAdjacentRoom(0); 
                break;

            case 2: 
                ShowRoom(2); 
                ShowAdjacentRoom(0); 
                ShowAdjacentRoom(3);
                break;

            case 3: 
                ShowRoom(3); 
                ShowAdjacentRoom(4); 
                ShowAdjacentRoom(6); 
                break;

            case 4: //room 5
                ShowRoom(4); 
                ShowAdjacentRoom(5); 
                break;

            case 6: 
                ShowRoom(6); 
                ShowAdjacentRoom(7); 
                break;
        }
    }

    void ShowRoom(int roomIndex)
    {
        roomMapIcons[roomIndex].SetActive(true); 

        if (visitedRooms[roomIndex])
        {
            roomMapIcons[roomIndex].GetComponent<SpriteRenderer>().color = Color.white;
        }
        
    }

    void ShowAdjacentRoom(int roomIndex)
    {
        roomMapIcons[roomIndex].SetActive(true);
        if (!visitedRooms[roomIndex])
        {
            roomMapIcons[roomIndex].GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }
}
