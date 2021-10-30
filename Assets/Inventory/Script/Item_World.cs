using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_World : MonoBehaviour
{
    [Header("Sprite For Key To Unlock")]
    [SerializeField]public SpriteRenderer SRenderer;
    [SerializeField]private Sprite[] Sprites; 
    public CharacterMouvement characterMouvement;
    public bool DoppioSalto , Scaling , SaltoCalibrato ;
    


    private void Start()
    {
        if(DoppioSalto == true)
        {
            SRenderer.sprite = Sprites[0];
        }
        if(Scaling == true)
        {
            SRenderer.sprite = Sprites[1];
        }
        if (SaltoCalibrato == true)
        {
            SRenderer.sprite = Sprites[2];
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (DoppioSalto == true && other.CompareTag("Player"))
        {
            characterMouvement.b_Doublejump_key = true;
            Destroy(gameObject, 1f);
        }
        if (Scaling == true && other.CompareTag("Player"))
        {
            characterMouvement.wallJumpingKey = true;
            Destroy(gameObject, 1f);
        }
        if (SaltoCalibrato == true && other.CompareTag("Player"))
        {
            characterMouvement.CalibratedJumping_Key = true;
            Destroy(gameObject, 1f);
        }
    }
}
