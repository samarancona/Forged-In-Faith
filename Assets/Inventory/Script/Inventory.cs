using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    public event EventHandler OnItemListChanged;
    private List<Item> itemList;
    private Action<Item> useItemAction;
    public Inventory(Action<Item> useItemAction)
    {
        this.useItemAction = useItemAction;
        itemList = new List<Item>();
        AddItem(new Item { itemType = Item.Itemtype.coin, Amount = 1 });
        AddItem(new Item { itemType = Item.Itemtype.HealthPosion, Amount = 1 });
        AddItem(new Item { itemType = Item.Itemtype.medkit, Amount = 1 });
        //AddItem(new Item { ItemType = Item.Itemtype.HealthPosion, Amount = 1 });
    }
    public void AddItem (Item item)
    {
        if (item.IsStackable())
        {
            bool ItemAlreadyInInventory = false;
            foreach(Item inventoryItem in itemList)
            {
                if(inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.Amount += item.Amount;
                    ItemAlreadyInInventory = true;
                }
            }
            if (!ItemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }
        
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item ItemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.Amount -= item.Amount;
                    ItemInInventory = inventoryItem;
                }
            }
            if (ItemInInventory != null && ItemInInventory.Amount <= 0)
            {
                itemList.Remove(ItemInInventory);
            }
        }
        else
        {
            itemList.Remove(item);
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
    public void UseItem(Item item)
    {
        useItemAction(item);
    }
}
