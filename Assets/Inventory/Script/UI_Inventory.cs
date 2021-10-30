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
        
        StartCoroutine(Wait_instantiating());
        
        
    }
    IEnumerator Wait_instantiating()
    {
        yield return new WaitForSeconds(0f);
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTamplate = itemSlotContainer.transform.Find("itemSlotTamplate");
        Debug.Log(itemSlotTamplate);
        
    }
    public void Set_inventory(Inventory inventory)
    {
        this.inventory = inventory;

        RefreshInventoriesItem();
        
    }



    private void RefreshInventoriesItem()
    {
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 30f;
        foreach (Item item in inventory.GetItemList())
        {
            Debug.Log(item.ItemType);
            Debug.Log("sono entrato nel foreach");
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTamplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            
            x++;
            if (x > 4)
            {
                x = 0;
                y++;
            }
        }
    }
}
