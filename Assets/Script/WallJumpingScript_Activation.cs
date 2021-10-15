using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpingScript_Activation : MonoBehaviour
{
    public CharacterController2D characterController2D;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            characterController2D.WallSlidingKey = true;
            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("salto");
                characterController2D.WallJumpingKey = true;
            }

        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            characterController2D.WallSlidingKey = false;
            characterController2D.WallJumpingKey = false;
            
        }
    }
}
