using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool IsVisited = false;
    public LayerMask playerLayer;
    public int roomNumber;
    public LayerMask enemyLayer;
    public GameObject EnemyToGenerate;
    public int NumberOfEnemies;
    public bool isShop;

    public Vector2 Position
    {
        get { return new Vector2(transform.position.x, transform.position.y); }
    }

    void Start()
    {
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
        if (!isShop && !IsVisited) {
            for (int i = 0; i < NumberOfEnemies; i++)
            {
                if (EnemyToGenerate != null)
                {
                    GameObject enemy = Instantiate(EnemyToGenerate, GetRandomPositionInRoom(), Quaternion.identity);
                    Enemy enemyScript = enemy.GetComponent<Enemy>();

                    if (enemyScript != null)
                    {
                        enemyScript.room = roomNumber; 
                    }
                }
            }
        }
        IsVisited = true;
    }

    Vector2 GetRandomPositionInRoom()
    {
        float centerX = transform.position.x;
        float centerY = transform.position.y;
        // Reduce the range to spawn enemies closer to the center of the room
        float randomX = Random.Range(centerX - 2, centerX + 2);
        float randomY = Random.Range(centerY - 2, centerY + 2);
        return new Vector2(randomX, randomY);
    }
}
