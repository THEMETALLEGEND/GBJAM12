using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<ShopItens> items = new List<ShopItens>();

    void Start()
    {
        // Apenas um exemplo de como acessar os itens
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
