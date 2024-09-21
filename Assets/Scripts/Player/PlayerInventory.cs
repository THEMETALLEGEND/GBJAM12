using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<ShopItens> Inventory = new List<ShopItens>();
    [HideInInspector] public bool hasKey = false;
    public int money;
    private ShopManager shop_Manager;
    public AudioClip coinPicked;
    private AudioSource sourc_;
    public GameObject UI_Controller;

    void Start()
    {
        sourc_ = GetComponent<AudioSource>();
        shop_Manager = FindObjectOfType<ShopManager>();
        UI_Controller.GetComponent<UI_Controller>().UpdateCoins(money);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Collectibles"))
        {
            GameObject coin = coll.gameObject;
            money++;
            Destroy(coin);
            sourc_.clip = coinPicked;
            sourc_.Play();
            UI_Controller.GetComponent<UI_Controller>().UpdateCoins(money);

        }
        else if (coll.CompareTag("Shop Itens")) 
        {
            ShopItens item = shop_Manager.GetItemById(coll.GetComponent<Item>().itemId); // finds item in ShopManager list by the id of the Item.cs on the object.
            if (item.price <= money) { // see if the player has the money to pay for it
                money -= item.price;
                CollectItem(item); //the switch where the item effects will really be applied
                Destroy(coll.gameObject);
                UI_Controller.GetComponent<UI_Controller>().UpdateCoins(money);
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
                if(playerObject.GetComponent<PlayerAttacking>().hp < 6)
                {
                    Ui.GetComponent<UI_Controller>().UpdateHeartStates(Ui.GetComponent<UI_Controller>().life + 1);
                }
                else
                {
                    playerObject.GetComponent<PlayerAttacking>().hp = 6;
                    money += item.price;
                }
                
                break; 
        }
    }
}
