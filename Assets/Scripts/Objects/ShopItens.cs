using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItens : MonoBehaviour
{
    //class to store data.
    public int id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int price {  get; set; }
    public ShopItens(int id, string name, string description, int price)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.price = price;
    }

}
