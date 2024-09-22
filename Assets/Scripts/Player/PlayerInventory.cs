using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GBTemplate;

public class PlayerInventory : MonoBehaviour
{
    public List<ShopItens> Inventory = new List<ShopItens>();
    [HideInInspector] public bool hasKey = false;
    public int money;
    private ShopManager shop_Manager;
    public AudioClip coinPicked;
    private GBSoundController soundController; 
    public GameObject UI_Controller;

    void Start()
    {
        soundController = FindObjectOfType<GBSoundController>(); 
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
            soundController.PlaySound(coinPicked); 
            UI_Controller.GetComponent<UI_Controller>().UpdateCoins(money);
        }
        else if (coll.CompareTag("Shop Itens"))
        {
            ShopItens item = shop_Manager.GetItemById(coll.GetComponent<Item>().itemId); 
            if (item.price <= money) 
            {
                money -= item.price;
                CollectItem(item); 
                Destroy(coll.gameObject);
                UI_Controller.GetComponent<UI_Controller>().UpdateCoins(money);
            }
        }
    }

    void CollectItem(ShopItens item)
    {
        Debug.Log($"Collected: {item.itemName}, Description: {item.description}");
        Inventory.Add(item);
        switch (item.id)
        {
            case 0:
                PlayerAttacking playerObject = FindObjectOfType<PlayerAttacking>();
                if (playerObject != null)
                    playerObject.GetComponent<PlayerAttacking>().hp++;
                UI_Controller ui = FindObjectOfType<UI_Controller>();
                if (playerObject.GetComponent<PlayerAttacking>().hp < 6)
                {
                    ui.GetComponent<UI_Controller>().UpdateHeartStates(ui.GetComponent<UI_Controller>().life + 1);
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
