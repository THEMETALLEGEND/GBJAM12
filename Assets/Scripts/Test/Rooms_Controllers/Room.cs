using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool IsVisited = false;
    public LayerMask playerLayer;
    public int roomNumber;
    public LayerMask enemyLayer;
    public bool isShop;

    private BoxCollider2D roomCollider;

    private void Start()
    {
        roomCollider = GetComponent<BoxCollider2D>();
        AssignRoomNumbers();
    }

    void AssignRoomNumbers()
    {
        Room[] allRooms = FindObjectsOfType<Room>();

        System.Array.Sort(allRooms, (a, b) =>
        {
            if (a.transform.position.x == b.transform.position.x)
            {
                return a.transform.position.y.CompareTo(b.transform.position.y);
            }
            return a.transform.position.x.CompareTo(b.transform.position.x);
        });

        for (int i = 0; i < allRooms.Length; i++)
        {
            allRooms[i].roomNumber = i + 1;
        }
    }

    public void GenerateEnemies()
    {
        if (!isShop && !IsVisited)
        {
            IsVisited = true;
            FindAndActivateSpawners();
        }
    }

    void FindAndActivateSpawners()
    {
        Collider2D[] spawnersInRoom = Physics2D.OverlapBoxAll(roomCollider.bounds.center, roomCollider.bounds.size, 0f, LayerMask.GetMask("EnemySpawner"));

        foreach (Collider2D spawnerCollider in spawnersInRoom)
        {
            Debug.Log("there is one spawner");
            EnemySpawner enemySpawner = spawnerCollider.GetComponent<EnemySpawner>();
            if (enemySpawner != null)
            {
                enemySpawner.roomNumber_ = roomNumber; 
                enemySpawner.SpawnEnemies();
            }
        }
    }
}
