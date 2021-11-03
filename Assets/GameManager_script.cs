using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_script : MonoBehaviour
{
    [Header("For the UI")]
    [SerializeField]private GameObject UI_ItemContainer;
    [SerializeField]private GameObject UI_BeckgroundInv;
    bool b_ItemContainer = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.AltGr))
        {
            b_ItemContainer = !b_ItemContainer;
            UI_ItemContainer.SetActive(b_ItemContainer);
            UI_BeckgroundInv.SetActive(b_ItemContainer);
        }
    }

}
