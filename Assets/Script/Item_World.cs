using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_World : MonoBehaviour
{
    
    public CharacterMouvement characterMouvement;
    public bool DoppioSalto = false, Scaling = false, SaltoCalibrato = false;
        
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (DoppioSalto == true && other.CompareTag("Player"))
        {
            characterMouvement.b_Doublejump_key = true;
        }
    }
}
