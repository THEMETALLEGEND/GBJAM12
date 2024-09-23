using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<ShopItens> items = new List<ShopItens>();
    public int price_SmallHeart;
    public int price_Heart;
    public int price_RNG_Upgrade;
    public int price_ROF_Upgrade;

    void Start()
    {
        items.Add(new ShopItens(0, "Small Heart", "", price_SmallHeart));
        items.Add(new ShopItens(1, "Heart", "", price_Heart));
        items.Add(new ShopItens(2, "RangeUpgrade", "", price_RNG_Upgrade));
        items.Add(new ShopItens(3, "FireRateUpgrade", "", price_ROF_Upgrade));


        foreach (var item in items)
        {
            Debug.Log($"Item: {item.itemName}, Description: {item.description}, Price: {item.price}");
        }
    }

    public ShopItens GetItemById(int id)
    {
        return items.Find(item => item.id == id);
    }
}
