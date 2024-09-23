using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Para verificar a cena atual
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
    public float rangeIncreaseOnUpgrade = 1f;
    public float CooldownTimeToDecrease = 0.3f;
    private float originalRange;
    private float originalCooldownTime;

    void Start()
    {
        shop_Manager = FindObjectOfType<ShopManager>();
        UI_Controller.GetComponent<UI_Controller>().UpdateCoins(money);

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            PlayerPrefs.SetInt("actualHP", 6);
            if (PlayerPrefs.HasKey("originalRange"))
            {
                gameObject.GetComponent<PlayerAttacking>().range = PlayerPrefs.GetFloat("originalRange");
            }
            if (PlayerPrefs.HasKey("originalCooldown"))
            {
                gameObject.GetComponent<PlayerAttacking>().magicCooldownTime = PlayerPrefs.GetFloat("originalCooldown");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        PlayerAttacking playerObject = FindObjectOfType<PlayerAttacking>();
        soundController = FindObjectOfType<GBSoundController>();
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
        PlayerAttacking playerObject = FindObjectOfType<PlayerAttacking>();
        UI_Controller ui = FindObjectOfType<UI_Controller>();

        Inventory.Add(item);

        switch (item.id)
        {
            case 0:
                if (playerObject != null)
                    playerObject.GetComponent<PlayerAttacking>().hp++;
                if (playerObject.GetComponent<PlayerAttacking>().hp < 6)
                {
                    ui.UpdateHeartStates(playerObject.GetComponent<PlayerAttacking>().hp);
                }
                else
                {
                    playerObject.GetComponent<PlayerAttacking>().hp = 6;
                    money += item.price;
                }
                PlayerPrefs.SetInt("actualHP", playerObject.GetComponent<PlayerAttacking>().hp);
                break;

            case 1:
                if (playerObject != null)
                    playerObject.GetComponent<PlayerAttacking>().hp += 2;
                if (playerObject.GetComponent<PlayerAttacking>().hp < 6)
                {
                    ui.UpdateHeartStates(playerObject.GetComponent<PlayerAttacking>().hp);
                }
                else
                {
                    playerObject.GetComponent<PlayerAttacking>().hp = 6;
                    money += item.price;
                }
                PlayerPrefs.SetInt("actualHP", playerObject.GetComponent<PlayerAttacking>().hp);
                break;

            case 2:

                if (!PlayerPrefs.HasKey("originalRange"))
                {
                    PlayerPrefs.SetFloat("originalRange", gameObject.GetComponent<PlayerAttacking>().range);
                }

                gameObject.GetComponent<PlayerAttacking>().range += rangeIncreaseOnUpgrade;
                PlayerPrefs.SetFloat("rangeUpgrade", rangeIncreaseOnUpgrade);
                PlayerPrefs.Save();
                break;

            case 3:

                if (!PlayerPrefs.HasKey("originalCooldown"))
                {
                    PlayerPrefs.SetFloat("originalCooldown", gameObject.GetComponent<PlayerAttacking>().magicCooldownTime);
                }

                gameObject.GetComponent<PlayerAttacking>().magicCooldownTime -= CooldownTimeToDecrease;
                PlayerPrefs.SetFloat("magicCooldown", CooldownTimeToDecrease);
                PlayerPrefs.Save();
                break;
        }
    }
}