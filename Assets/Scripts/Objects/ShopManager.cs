using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<ShopItens> items = new List<ShopItens>();

    void Start()
    {
        //here's where we are gonna input all the items. it is classified by id, name, description, and then price.
        items.Add(new ShopItens(0, "Heart", "Increases health.", 1));
    }

    public ShopItens GetItemById(int id)
    {
        return items.Find(item => item.id == id);
    }
}
