[System.Serializable]
public class Item
{
    public string Name;
    public int Id = -1;
    public Item()
    {
        Name = "";
        Id = -1;
    }
    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.data.Id;
    }
}
[System.Serializable]
public class ItemCraft
{
    public ItemObject resourceItem;
    public int sourceNumber;
}
