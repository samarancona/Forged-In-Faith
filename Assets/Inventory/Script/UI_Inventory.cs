using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTamplate;
    

    
    private void Awake()
    {

        //StartCoroutine(Wait_instantiating());
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTamplate = itemSlotContainer.GetChild(0).transform;      //.Find("itemSlotTamplate");
        //Debug.Log(itemSlotTamplate.name);

    }
    /*IEnumerator Wait_instantiating()
    {
        yield return new WaitForSeconds(0f);
        
        
    }*/
    public void Set_inventory(Inventory inventory)
    {
        this.inventory = inventory;

        
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoriesItem();
        
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoriesItem();
    }

    



    private void RefreshInventoriesItem()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTamplate) continue;
            Destroy(child.gameObject);
        }
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 200f;
        foreach (Item item in inventory.GetItemList())
        {
            
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTamplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();
            
            x++;
            if (x > 4)
            {
                x = 0;
                y--;
            }
        }
    }
}
