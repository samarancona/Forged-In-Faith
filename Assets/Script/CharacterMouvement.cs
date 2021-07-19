using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMouvement : MonoBehaviour
{
    public CharacterController2D Controller2D; 
    public float Speed = 40f;
    private float HorizontalMove = 0f;
    private bool jump;
    private float JumpTime = 0f;
    [SerializeField] float CalibratedJumpForce = 0f;
    
    Animator m_Animator;
    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        ////inizializzo variabili salto
        //if (Input.GetButtonDown("Jump"))
        //{
            
        //    Controller2D.m_JumpForce = 600f;
        //}
        //caricare il salto 
        if (Input.GetButton("Jump"))
        {
            JumpTime += 1f;
            if(JumpTime == 40f)
            {
                CalibratedJumpForce += 400f;
            }
            //if(JumpTime == 80f)               
            //{                                                      
            //    CalibratedJumpForce += 500f;               // solo se si vuole un'altro salto calibrato più potente
            //}
            
            Debug.Log(JumpTime);

        }
        // il salto è dato dalla somma delle due varibili 
        if (Input.GetButtonUp("Jump"))
        {
            Controller2D.m_JumpForce = 600f;    //Reset Variable
            Controller2D.m_JumpForce += CalibratedJumpForce;
            jump = true;
            CalibratedJumpForce = 0f;           //Reset Variable 
            JumpTime = 0f;                      //Reset Variable
            
            Debug.Log(Controller2D.m_JumpForce);
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
        //if (Controller2D.b_DoubleJump && !Controller2D.m_Grounded && Input.GetButton("Jump"))
        //{
                                                                                                      // se si vuole un'animazione di capovolta al doppio salto 
        //    m_Animator.Play("Base Layer.DoubleJumpFlip", 0, 0f);
        //}

        else
        {
            m_Animator.SetBool("Run", false);
        }
    }


}
