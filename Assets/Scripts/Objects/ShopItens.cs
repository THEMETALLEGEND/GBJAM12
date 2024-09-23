[System.Serializable]
public class ShopItens
{
    public int id {  get; set; }
    public string itemName { get; set; }
    public string description { get; set; }
    public int price { get; set; }
    public ShopItens(int id, string itemName, string description, int price)
    {
        this.id = id;
        this.itemName = itemName;
        this.description = description;
        this.price = price;
    }
}

