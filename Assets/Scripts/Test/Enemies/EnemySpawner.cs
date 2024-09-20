using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyToGenerate;
    public int roomNumber_;
    bool alreadyGenerated = false;
    public void SpawnEnemies()
    {
        Debug.Log("room number "+roomNumber_);
        if (EnemyToGenerate != null)
        {
            StartCoroutine(GenerateRoutine());
        }
    }
    private IEnumerator GenerateRoutine()
    {
        if (!alreadyGenerated)
        {
            alreadyGenerated = true;
            if (EnemyToGenerate != null)
            {
                GameObject enemy = Instantiate(EnemyToGenerate, transform.position, Quaternion.identity);
                enemy.GetComponent<Enemy>().SetRoom(roomNumber_);
            }
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}
