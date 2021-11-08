using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMouvement : MonoBehaviour
{
    [Header("For Player")]
    [SerializeField] public float HP_Player = 100f;
    [SerializeField] public float EP_Player = 100f;
    [SerializeField] public float ProgressBarPoints = 0f;
    private Item_World m_ItemToPickUp;

    
   //private int PlayerDifferentForms = 5;



   [Header("Externals Variables")]
    public GameObject TipGrab_UI;
    public CharacterController2D Controller2D;
    public UI_Inventory uiInventory;
    private Inventory inventory;
    

    [Header("For Mouvement")]
    [SerializeField] private float FallMultiplayer = 0f;
    [SerializeField] public float Speed;
    [SerializeField] private bool canMove = true;
    private float OriginalSpeed;
    private float HorizontalMove = 0f;
    private float OriginalFallVelocity;






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
    public float FallWJVelocity;
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
    private bool WallJumping = false;
    private bool WallSliding;




    [Header("For Gliding")]
    [SerializeField] public bool GlidingKey = false;
    public GameObject AliPlaceHolder;
    public float FallGlideVelocity;
    [SerializeField] public float GlideTime = 0.5f;
    private bool b_CanGlide = false;
    private float CounterGlide = 0.5f;




    //...



    //Declating Variable Components Player
    private Rigidbody2D s_rigidbody2D;
    Animator m_Animator;
    
    
    

    private void Start()
    {
        HP_Player = 100f;
        
        OriginalSpeed = Speed;
        OriginalFallVelocity = FallMultiplayer;
        m_Animator = GetComponent<Animator>();
        s_rigidbody2D = GetComponent<Rigidbody2D>();
        M_grounded = Controller2D.m_Grounded;
        inventory = new Inventory(UseItem);
        uiInventory.Set_inventory(inventory);
    }


    private void UseItem(Item item)
    {
        switch (item.itemType) 
        {
            case Item.Itemtype.HealthPosion:
                Debug.Log("mi sto curando");
                inventory.RemoveItem(new Item { itemType = Item.Itemtype.HealthPosion, Amount = 1 });
                break;
            case Item.Itemtype.medkit:
                Debug.Log("mi sto super curando cazzo");
                inventory.RemoveItem(new Item { itemType = Item.Itemtype.medkit, Amount = 1 });
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {

        M_grounded = Controller2D.m_Grounded;

        HorizontalMove = Input.GetAxisRaw("Horizontal");

        Input_JumpKeyDown = Input.GetButtonDown("Jump");





        //for Pick The Item
        if(Input.GetKeyDown(KeyCode.E) && m_ItemToPickUp != null)
        {
            inventory.AddItem(m_ItemToPickUp.GetItem());
            Debug.Log("preso!!!!!!!");
            m_ItemToPickUp.DestroySelf();
        }

        //chuamo ad ogni frame la funzione che controlla ad ogni frame se sto toccando l'oggetto (layer) giusto
        OverlappingFunction();


        //chiamo la funzione per impostare variabili salto
        JumpingFunction();



        //imposto ad ogni frame la direzione di wall jump
        SetWallJumpDirection();

        //--------------------------------------------------------//
        if (Input.GetKey(KeyCode.Space))
        {
            CheckWallSliding();

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {

        }

        //chiamo wall jumping (setto variabili che servono a elabprare info in fixed update )
        WallJumpingFunction();

        

        //inutile commentare
        //CheckAnimation();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item_World item_World = collision.GetComponent<Item_World>();
        if (item_World != null)
        {
            TipGrab_UI.SetActive(true);
            m_ItemToPickUp = item_World;
            // toucing Item and controll if press E
            
            
            
        }
    }
    

    

    private void OnTriggerExit2D(Collider2D collision)
    {
        Item_World item_World = collision.GetComponent<Item_World>();
        if (item_World != null)
        {
            m_ItemToPickUp = null;
            TipGrab_UI.SetActive(false);
        }
    }



    






    private void FixedUpdate()
    {

        // CONTROLLO SE HO PRESO LA MECCANICKEY NEL MONDO PER poi chiamare la funzione di gliding
        if (GlidingKey) { GlidingFunction(b_Doublejump); }

        //-----------------------------------------------------//





        //controllo la FALLVELOCITY
        if (s_rigidbody2D.velocity.y < 0) { FallVelocityFunction(); }

        //_--------------------------------------------------------//

        

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

        




        /////////////////////////////////////////
        if (canMove)
        {
            if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)) && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)))
            {

                Controller2D.Move(HorizontalMove * Speed / 2 * Time.fixedDeltaTime);

            }
            else
            {
                Controller2D.Move(HorizontalMove * Speed * Time.fixedDeltaTime);

            }
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



        if (IsJumping == true               /*Input_JumpKeyDown && M_grounded == true*/)
        {
            m_Animator.SetBool("Jump", true);
            
        }
        if (M_grounded == true || s_rigidbody2D.velocity.y <= 0)
        {
            m_Animator.SetBool("Jump", false);
        
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
                //Debug.Log("doppio salto");
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



    private void FallVelocityFunction()
    {
        
        
        s_rigidbody2D.velocity = Vector2.up  * (s_rigidbody2D.velocity.y + Physics2D.gravity.y) / FallMultiplayer ;//* Time.deltaTime
        //Debug.Log(s_rigidbody2D.velocity.y);
        
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
                Debug.Log(FallMultiplayer);
                
                
            }
        }

        //se smetto di tenere premuto il tasto per glidare allora disattivo variabili e oggetti riferiti al gliding
        if (Input.GetButtonUp("Jump") || M_grounded)
        {
            b_CanGlide = false;
            FallMultiplayer = OriginalFallVelocity;
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
        
        
        if (/*Input_JumpKeyDown &&*/ WallSliding == true)
        {
            WallJumping = true;

            
            //Invoke("SetWallJumpingToFalse", WallJumpTime);
        }
        //else
        //{
        //    WallJumping = false;
        //}

        if (M_grounded || s_rigidbody2D.velocity.y <= 0f)
        {
            FallMultiplayer = OriginalFallVelocity;
            Speed = OriginalSpeed;
        }

        if ( /*WallJumping == true && */wallJumpingKey == true)
        {
            MakeWallJump();

            //forse da fare in cooroutine
        }
        //JumpingWallFunction end
    }

    private void CheckWallSliding()
    {
        if (isToucingFront  && !M_grounded && s_rigidbody2D.velocity.y < 0 )
        {
            canMove = false;
            WallSliding = true;
            b_Doublejump = false;

            
            //else
            //{
            //    WallSliding = false;
            //}

        }
        else if(!isToucingFront && M_grounded && s_rigidbody2D.velocity.y > 0)
        {
            canMove = true;
            WallSliding = false;
            

        }
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
        if (!M_grounded && WallSliding)
        {
            if (Input.GetKeyUp(KeyCode.Space) && isToucingFront)
            {
                FallMultiplayer = FallWJVelocity;
                Speed = 50f;
                s_rigidbody2D.velocity = new Vector2(-xWallForce * WallJumpDirection, yWallForce);
                Debug.Log("walljump!");
                StartCoroutine(WallJumpCoolDownJ());


            }
        }
        

        



        //s_rigidbody2D.gravityScale = 5f;

    }

    IEnumerator WallJumpCoolDownJ()
    {
        canMove = false;
        
        yield return new WaitForSeconds(0.25f);

        canMove = true;

        
    }














    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(FrontCheck.position, 0.4f) ;
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(Controller2D.m_GroundCheck.position, 0.45f);
    }
}
