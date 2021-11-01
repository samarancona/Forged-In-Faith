using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_World : MonoBehaviour
{


    [Header("ItemToPickUp")]
    Item_Assets item_Assets;
    private SpriteRenderer SRenderer;
    private Item item;
    public bool HealthPostion = false, coin= false, medkit= false;

    private void Awake()
    {
        item_Assets = GameObject.FindWithTag("AssetsGameObject").GetComponent<Item_Assets>();
        SRenderer = GetComponent<SpriteRenderer>();
        Item_World item_World = transform.GetComponent<Item_World>();
        
        if (HealthPostion == true)
        {
            ///// LATER... dovra fare qualcosa 
            ///


            //imposto tipo Item
            this.item = new Item { itemType = Item.Itemtype.HealthPosion, Amount = 1 };
            Debug.Log(item.itemType);
            SRenderer.sprite = item_Assets.HealthPosionSprite; 
           
            //item.GetSprite();
            //item_World.SetItem(new Item { itemType = Item.Itemtype.HealthPosion , Amount = 1});


            //imposto sprite
            //SRenderer.sprite = item.GetSprite();          //Item_Assets.Instance.HealthPosionSprite;
        }
        if (coin == true)
        {

            //imposto tipo Item
            this.item = new Item { itemType = Item.Itemtype.coin, Amount = 1 };
            Debug.Log(item.itemType);
            SRenderer.sprite = item_Assets.CoinSprite;
            //item.GetSprite();
            //item_World.SetItem(new Item { itemType = Item.Itemtype.coin, Amount = 1 });


            //SRenderer.sprite = item.GetSprite();          //Item_Assets.Instance.CoinSprite;
        }
        if (medkit == true)
        {

            //imposto tipo Item
            this.item = new Item { itemType = Item.Itemtype.medkit, Amount = 1 };
            Debug.Log(item.itemType);
            SRenderer.sprite = item_Assets.Medkit;
            //item.GetSprite();
            //item_World.SetItem(new Item { itemType = Item.Itemtype.medkit, Amount = 1 });

            //SRenderer.sprite = item.GetSprite();          //Item_Assets.Instance.Medkit;
        }

    }
    public void SetItem(Item item)
    {
        this.item = item;
        Debug.Log(item.itemType);
        SRenderer.sprite = item.GetSprite();
    }
    public Item GetItem()
    {
        return item;
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
