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
		Debug.Log("room number " + roomNumber_);
		if (EnemyToGenerate != null)
		{
			StartCoroutine(GenerateRoutine());
		}
	}

	private void Awake()
	{
		SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		spriteRenderer.enabled = false;
	}

	private IEnumerator GenerateRoutine()
	{
		if (!alreadyGenerated)
		{
            alreadyGenerated = true;
            yield return new WaitForSeconds(2f);
			if (EnemyToGenerate != null)
			{
				GameObject enemy = Instantiate(EnemyToGenerate, transform.position, Quaternion.identity);
				enemy.gameObject.SetActive(true);
				enemy.GetComponent<Enemy>().SetRoom(roomNumber_);
			}

			Destroy(gameObject);
		}
	}
}
