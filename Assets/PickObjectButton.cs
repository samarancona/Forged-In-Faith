using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PickObjectButton : MonoBehaviour
{
    [SerializeField]GameObject ObjectToPick;
    
    public void OnPicked()
    {

        Destroy(ObjectToPick);
        
        Debug.Log("PRESO");
    }

}
