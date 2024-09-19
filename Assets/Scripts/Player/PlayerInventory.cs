using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
	[HideInInspector] public bool hasKey = false;
    public int money;
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Collectibles"))
        {
            GameObject coin = coll.gameObject;
            money++;
            Destroy(coin);
        }
    }
}
