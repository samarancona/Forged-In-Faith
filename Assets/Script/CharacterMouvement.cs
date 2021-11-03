using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMouvement : MonoBehaviour
{
    [Header("Externals Variables")]
    public CharacterController2D Controller2D;
    public UI_Inventory uiInventory;
    private Inventory inventory;


    [Header("For Mouvement")]
    [SerializeField] private float FallMultiplayer = 0f;
    [SerializeField] public float Speed = 40f;
    private float HorizontalMove = 0f;
    private float FallVelocity;






    [Header("For Jumping")]
    [SerializeField] public bool b_Doublejump_key = false;
    [SerializeField] public bool CalibratedJumping_Key;
    [SerializeField] float DoubleJumpForce = 0f;
    [SerializeField] public float JumpTime = 0.35f;
    [SerializeField] public float JumpForce = 0f;
    private bool b_Doublejump = false;
    private bool IsJumping;
    private float CounterJump;
    private bool M_grounded;
    private bool Input_JumpKeyDown = false;





    [Header("For Wall Jumping")]
    public bool wallJumpingKey = false;
    public Transform FrontCheck;
    public float WallSlidingSpeed = 0;
    public LayerMask wallJumpLayer;
    public float xWallForce;
    public float yWallForce;
    public float WallJumpTime;
    private float WallJumpDirection = -1f;
    private bool isToucingFront;
    const float k_FrontCheckRadius = 0.4f;                     // Radius of the overlap circle to determine if there is something in front
    private bool WallJumping;
    private bool WallSliding;


    [Header("For Gliding")]
    [SerializeField] private GameObject AliPlaceHolder;
    [SerializeField] private float FallGlideVelocity = -4;
    [SerializeField] public float GlideTime = 0.5f;
    private bool b_CanGlide = false;
    private float CounterGlide = 0.5f;




    //...



    //Declating Variable Components Player
    private Rigidbody2D s_rigidbody2D;
    Animator m_Animator;
    
    
    

    private void Start()
    {
        FallVelocity = FallMultiplayer;
        m_Animator = GetComponent<Animator>();
        s_rigidbody2D = GetComponent<Rigidbody2D>();
        M_grounded = Controller2D.m_Grounded;
        inventory = new Inventory();
        uiInventory.Set_inventory(inventory);
    }








    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item_World item_World = collision.GetComponent<Item_World>();
        if(item_World != null)
        {
            // toucing Item
            inventory.AddItem(item_World.GetItem());
            Debug.Log("preso!!!!!!!");
            item_World.DestroySelf();
        }
    }








    // Update is called once per frame
    void Update()
    {
        M_grounded = Controller2D.m_Grounded;
        HorizontalMove = Input.GetAxisRaw("Horizontal");
        Input_JumpKeyDown = Input.GetButtonDown("Jump");



        OverlappingFunction();


        

        


        JumpingFunction();


        WallJumpingFunction();


        GlidingFunction(b_Doublejump);



        CheckAnimation();
    }
    






    private void FixedUpdate()
    {
        ControllFallVelocity();


        SetWallJumpDirection();

        //fixed fixis mouvement for wall jumping
        if (WallSliding)
        {
            //s_rigidbody2D.gravityScale = 0f;
            s_rigidbody2D.velocity = new Vector2(s_rigidbody2D.velocity.x, Mathf.Clamp(s_rigidbody2D.velocity.y, -WallSlidingSpeed, float.MaxValue));////0f

        }
        /*else if (WallSliding == false)
        {
            s_rigidbody2D.gravityScale = 5f;
        }
        */

        if (WallJumping == true && wallJumpingKey == true)
        {
            MakeWallJump(); //forse da fare in cooroutine
        }

        
        

        /////////////////////////////////////////

        
        if ((Input.GetKey(KeyCode.UpArrow)|| Input.GetKey(KeyCode.DownArrow)) && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)))
        {
            
            Controller2D.Move(HorizontalMove * Speed /2 * Time.fixedDeltaTime);
            
        }
        else 
        {
            Controller2D.Move(HorizontalMove* Speed * Time.fixedDeltaTime);
            
        }
        

    }










    //funcition that set the animation boolean/key Variables
    private void CheckAnimation()
    {
        
        
        if (HorizontalMove < 0 || HorizontalMove > 0)
        {
            m_Animator.SetBool("Run", true);

        }
        else
        {
            m_Animator.SetBool("Run", false);
        }



        if (Input_JumpKeyDown && M_grounded == true)
        {
            m_Animator.SetBool("Jump", true);
            
        }
        if (M_grounded == true || s_rigidbody2D.velocity.y <= 0)
        {
            m_Animator.SetBool("Jump", false);
            
        }

        //double jump
        if (!M_grounded && Input_JumpKeyDown && b_Doublejump_key == true && b_Doublejump == true)
        {
            m_Animator.SetBool("DoubleJump", true);
            


        }
        else //if (M_grounded == true)
        {
            m_Animator.SetBool("DoubleJump", false);
        }





    }









    //Function Called in update for setting Variables (and Phisics DONT DO THIS!!!) for Jumping  ( LATER TO MAKE THE PHISICS IN FIXED UPDATE)
    private void JumpingFunction()
    {
                                                                   ////inizializzo variabili salto
        if (Input_JumpKeyDown && M_grounded == true)
        {

            
            

            IsJumping = true;
            CounterJump = JumpTime;
            s_rigidbody2D.velocity = Vector2.up * JumpForce;
            b_Doublejump = true;
            //Controller2D.m_JumpForce = 600f;
        }
        //double jump
        if (!M_grounded && Input_JumpKeyDown && b_Doublejump_key == true)
        {
            
            if (b_Doublejump == true )
            {

                s_rigidbody2D.velocity = Vector2.zero;                               // azzero la velocità verticale in modo da avere un salto con il valore non mutato
                s_rigidbody2D.velocity += Vector2.up * DoubleJumpForce;              //(-1 * Physics2D.gravity.y) * 1.5f;
                b_Doublejump = false;
                b_CanGlide = true;
                Debug.Log("doppio salto");
            }

        }
        //caricare il salto 
        if (Input.GetButton("Jump") && IsJumping == true && CalibratedJumping_Key == true)
        {

            if (CounterJump > 0)
            {
                s_rigidbody2D.velocity = Vector2.up * JumpForce;
                CounterJump -= Time.deltaTime;
                
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

        

    }



    private void ControllFallVelocity()
    {
        if (s_rigidbody2D.velocity.y < 0)
        {
            s_rigidbody2D.velocity = Vector2.up  * (s_rigidbody2D.velocity.y + Physics2D.gravity.y) / FallMultiplayer ;//* Time.deltaTime
            Debug.Log(s_rigidbody2D.velocity);
        }
    }



    

    private void GlidingFunction(bool doublejump)
    {
        //se premo il tengo il tasto premuto per glidare e ho fatto il doppio salto e sono in aria e in fine se posso effetivamente glidare allora dopo un certo tempo di pressione Voilà
        if (Input.GetButton("Jump") && !M_grounded && b_CanGlide)
        {

            CounterGlide -= Time.deltaTime;
            


            //timer Counter glide
            if(CounterGlide <= 0)
            {
                
                AliPlaceHolder.SetActive(true);
                FallMultiplayer = FallGlideVelocity;
                Debug.Log("Sto glidando di brutto-----");
                
            }
        }

        //se smetto di tenere premuto il tasto per glidare allora disattivo variabili e oggetti riferiti al gliding
        if (Input.GetButtonUp("Jump") || M_grounded)
        {
            b_CanGlide = false;
            FallMultiplayer = FallVelocity;
            AliPlaceHolder.SetActive(false);
            CounterGlide = GlideTime;
        }
    }





    //Function That makes all the overlapping functions
    void OverlappingFunction()
    {
        isToucingFront = Physics2D.OverlapCircle(FrontCheck.position, k_FrontCheckRadius, wallJumpLayer);
    }


//Function Called in update for setting All booleans Variables for Jumping or Sliding 
    private void WallJumpingFunction()
    {
        //JumpingWall Function
        
        if (isToucingFront == true && M_grounded == false)
        {
            WallSliding = true;
            b_Doublejump = false;
        }
        else
        {
            WallSliding = false;

        }
        if (Input_JumpKeyDown && WallSliding == true)
        {

            WallJumping = true;
            Invoke("SetWallJumpingToFalse", WallJumpTime);
        }


        //JumpingWallFunction end
    }





    //Funtion that set the WallJumping to false (Called in time obviously)
    void SetWallJumpingToFalse()
    {
        WallJumping = false;
    }






    //setting waLL jump direction in base at the Horizontal input
    void SetWallJumpDirection()
    {
        if(HorizontalMove < 0 )
        {
            WallJumpDirection = -1;
        }
        if(HorizontalMove > 0)
        {
            WallJumpDirection = 1;
        }
        
    }
    




    // Function that make the wall Jump(maybe better in cooroutine)
    void MakeWallJump()
    {
        
        s_rigidbody2D.velocity = new Vector2(xWallForce * -WallJumpDirection, yWallForce);
        //s_rigidbody2D.gravityScale = 5f;
        
    }
















    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(FrontCheck.position, 0.4f) ;
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(Controller2D.m_GroundCheck.position, 0.45f);
    }
}
