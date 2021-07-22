using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMouvement : MonoBehaviour
{
    
    public CharacterController2D Controller2D; 
    [SerializeField]public float Speed = 40f;
    private float HorizontalMove = 0f;
    private bool jump;
    [SerializeField] float DoubleJumpForce = 0f;
    private bool b_Doublejump;
    [SerializeField]public float JumpTime = 0.35f;
    [SerializeField]public float JumpForce = 0f;


    [SerializeField] float FallMultiplayer = 0f;
    private Rigidbody2D s_rigidbody2D;
    private bool IsJumping;
    float CounterJump;



    //[SerializeField] float CalibratedJumpForce = 0f;

    Animator m_Animator;
    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        s_rigidbody2D = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        JumpingFunction();
        


        
        

        // il salto è dato dalla somma delle due varibili 
        /*if (Input.GetButtonUp("Jump"))
        {
            Controller2D.m_JumpForce = 600f;    //Reset Variable
            Controller2D.m_JumpForce += CalibratedJumpForce;
            jump = true;
            CalibratedJumpForce = 0f;           //Reset Variable 
            JumpTime = 0f;                      //Reset Variable

            Debug.Log(Controller2D.m_JumpForce);
        }*/


        //if (Input.GetButton("Jump"))
        //{
        //    JumpTime += 1f;
        //    if (JumpTime == 40f)
        //    {
        //        CalibratedJumpForce += 400f;
        //        Controller2D.m_JumpForce = Controller2D.m_JumpForce + CalibratedJumpForce;
        //    }
        //    Debug.Log(JumpTime);
        //}
        //if (Input.GetButtonDown("Jump"))
        //{
        //    jump = true;

        //}
        //if (Input.GetButtonUp("Jump"))
        //{
        //    jump = false;
        //    JumpTime = 0f;
        //    CalibratedJumpForce = 0f;
        //    Controller2D.m_JumpForce = 600f;
        //}
        

    }
    private void FixedUpdate()
    {
        HorizontalMove = Input.GetAxisRaw("Horizontal") * Speed;
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
            Debug.Log("Mi sto Cagando a dosso ");
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

    private void JumpingFunction()
    {
        ////inizializzo variabili salto
        if (Input.GetButtonDown("Jump") && Controller2D.m_Grounded)
        {

            IsJumping = true;
            CounterJump = JumpTime;
            s_rigidbody2D.velocity = Vector2.up * JumpForce;
            b_Doublejump = true;
            //Controller2D.m_JumpForce = 600f;
        }

        if (!Controller2D.m_Grounded && Input.GetButtonDown("Jump"))
        {
            if (b_Doublejump == true)
            {
                s_rigidbody2D.velocity += Vector2.up * DoubleJumpForce;//(-1 * Physics2D.gravity.y) * 1.5f;
                b_Doublejump = false;
                Debug.Log("doppio salto");
            }

        }
        //caricare il salto 
        if (Input.GetButton("Jump") && IsJumping == true)
        {

            if (CounterJump > 0)
            {
                s_rigidbody2D.velocity = Vector2.up * JumpForce;
                CounterJump -= Time.deltaTime;
                Debug.Log("sto saltando più in alto ");
            }
            else
            {
                IsJumping = false;
            }


            //JumpTime += 1f;
            //if(JumpTime == 40f)
            //{
            //    CalibratedJumpForce += 200f;
            //}


            //if(JumpTime == 80f)               
            //{                                                      
            //    CalibratedJumpForce += 500f;               // solo se si vuole un'altro salto calibrato più potente
            //}
        }
        if (Input.GetButtonUp("Jump"))
        {
            IsJumping = false;
        }


        if (s_rigidbody2D.velocity.y < 0)
        {
            s_rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplayer - 1) * Time.deltaTime;
        }

    }
}
