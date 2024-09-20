using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<ShopItens> Inventory = new List<ShopItens>();
    [HideInInspector] public bool hasKey = false;
    public int money;
    private ShopManager shop_Manager;

    void Start()
    {
        shop_Manager = FindObjectOfType<ShopManager>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Collectibles"))
        {
            GameObject coin = coll.gameObject;
            money++;
            Destroy(coin);
        }
        else if (coll.CompareTag("Shop Itens")) 
        {
            ShopItens item = shop_Manager.GetItemById(coll.GetComponent<Item>().itemId); // finds item in ShopManager list by the id of the Item.cs on the object.
            if (item.price <= money) { // see if the player has the money to pay for it
                money -= item.price;
                CollectItem(item); //the switch where the item effects will really be applied
                Destroy(coll.gameObject);
            }
        }
    }

    void CollectItem(ShopItens item)
    {
        Debug.Log($"Collected: {item.name}, Description: {item.description}");
        Inventory.Add(item);
        switch(item.id)
        {
            case 0 :
                PlayerAttacking playerObject = FindObjectOfType<PlayerAttacking>();
                if (playerObject != null)
                    playerObject.GetComponent<PlayerAttacking>().hp++;
                UI_Controller Ui = FindObjectOfType<UI_Controller>();
                Ui.GetComponent<UI_Controller>().UpdateLife(false, 1);
                break; 
        }
    }
}
