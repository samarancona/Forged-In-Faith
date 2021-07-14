using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMouvement : MonoBehaviour
{
    public CharacterController2D Controller2D; 
    public float Speed = 40f;
    float HorizontalMove = 0f;
    bool jump;


    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButton("Jump"))
        {
            jump = true;
        }
        
        HorizontalMove = Input.GetAxisRaw("Horizontal") * Speed;
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow) && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)))
        {
            Controller2D.Move(HorizontalMove/2 * Time.fixedDeltaTime, false, jump);
            jump = false;
        }
        else
        {
            Controller2D.Move(HorizontalMove * Time.fixedDeltaTime, false, jump);
            jump = false;
        }
        
        
        
    }


}
