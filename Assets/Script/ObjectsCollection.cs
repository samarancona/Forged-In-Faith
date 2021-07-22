using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsCollection : MonoBehaviour
{
    [SerializeField]GameObject ObjectToPick;
    [SerializeField]GameObject ActiveButton;
    private bool entrato;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ActiveButton.SetActive(true);
        }
        entrato = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ActiveButton.SetActive(false);
        }
        entrato = false;
    }
    private void Update()
    {
        if(entrato== true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Destroy(ObjectToPick);
                Debug.Log("preso");
            }
        }
    }
}
