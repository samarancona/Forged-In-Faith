using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMouvement : MonoBehaviour
{
    public CharacterController2D Controller2D; 
    public float Speed = 40f;
    float HorizontalMove = 0f;
    bool jump;
    float JumpTime;
    float AdditionalJForce;
    
    Animator m_Animator;
    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        //inizializzo variabili salto
        if (Input.GetButtonDown("Jump"))
        {
            JumpTime = 0f;
            Controller2D.m_JumpForce = 300f;
        }
        //caricare il salto 
        if (Input.GetButton("Jump"))
        {
            JumpTime += 1f;
            if(JumpTime == 30f)
            {
                AdditionalJForce = 200f;
            }
            
            Debug.Log(JumpTime);

        }
        // il salto è dato dalla somma delle due varibili 
        if (Input.GetButtonUp("Jump"))
        {
            Controller2D.m_JumpForce += AdditionalJForce;
            jump = true;
            AdditionalJForce = 0;
        }

        HorizontalMove = Input.GetAxisRaw("Horizontal") * Speed;
    }
    private void FixedUpdate()
    {
        CheckAnimation();
        if ((Input.GetKey(KeyCode.UpArrow)|| Input.GetKey(KeyCode.DownArrow)) && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)))
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

    private void CheckAnimation()
    {
        if (Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.LeftArrow))
        {
            m_Animator.SetBool("Run", true);
        }
        else
        {
            m_Animator.SetBool("Run", false);
        }
    }


}
