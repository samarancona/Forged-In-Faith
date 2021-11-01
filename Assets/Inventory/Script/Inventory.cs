using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    public event EventHandler OnItemListChanged;
    private List<Item> itemList;

    public Inventory()
    {
        itemList = new List<Item>();
        AddItem(new Item { itemType = Item.Itemtype.coin, Amount = 1 });
        AddItem(new Item { itemType = Item.Itemtype.HealthPosion, Amount = 1 });
        AddItem(new Item { itemType = Item.Itemtype.medkit, Amount = 1 });
        //AddItem(new Item { ItemType = Item.Itemtype.HealthPosion, Amount = 1 });
    }
    public void AddItem (Item item)
    {
        itemList.Add(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
    public List<Item> GetItemList()
    {
        return itemList;
    }
}
