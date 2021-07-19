using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsCollection : MonoBehaviour
{
    [SerializeField]GameObject ActiveButton;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ActiveButton.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ActiveButton.SetActive(false);
        }
    }

}
