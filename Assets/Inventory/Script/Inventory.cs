using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    private List<Item> itemList;

    public Inventory()
    {
        itemList = new List<Item>();
        AddItem(new Item { ItemType = Item.Itemtype.coin, Amount = 1 });
        AddItem(new Item { ItemType = Item.Itemtype.HealthPosion, Amount = 1 });
    }
    public void AddItem (Item item)
    {
        itemList.Add(item);
    }
    public List<Item> GetItemList()
    {
        return itemList;
    }
}
