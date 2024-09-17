using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
	private PlayerInventory playerInventory;

	private void Awake()
	{
		playerInventory = GameObject.Find("Player").GetComponent<PlayerInventory>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && playerInventory.hasKey)
		{
			Debug.Log("Exited the level");
		}
	}
}
