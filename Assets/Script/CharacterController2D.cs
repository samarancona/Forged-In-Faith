using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	                                                     //Declaring Variables For The Wall Jump/Sliding
	
	
	[HideInInspector] public bool WallJumpingKey;
	public float xWallForce;
	public float yWallForce;
	public float wallJumpTime;
	public float WallSlidingSpeed;
	private bool IsTouchingFront;
	public Transform FrontCheck;
	private bool WallSliding;
	private float i_input;
	[HideInInspector]public bool WallSlidingKey = false;

	                                            // Boolean for the Double Jump Mechanism.

	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	public LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	public LayerMask m_WhatIsFront;                           // A mask determining what is in front to the character
	public Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings

	

	const float k_GroundedRadius = 0.73f;                      // Radius of the overlap circle to determine if grounded
	const float k_FrontCheckRadius = 1f;                     // Radius of the overlap circle to determine if there is something in front

	[HideInInspector]public bool m_Grounded;            // Whether or not the player is grounded.

	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up

	private Rigidbody2D m_Rigidbody2D;

	[HideInInspector]public bool m_FacingRight = true;  // For determining which way the player is currently facing.

	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	


	private void Awake()
	{
		
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
		
	}

	private void FixedUpdate()
	{
		i_input = Input.GetAxisRaw("Horizontal");
		WallJumpingFunction();
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject )
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
		
	}
	public static float CalculateJumpForce(float gravityStrength, float jumpHeight)
	{
		//h = v^2/2g
		//2gh = v^2
		//sqrt(2gh) = v
		return Mathf.Sqrt(2 * gravityStrength * jumpHeight);
	}
	

	public void Move(float move)
	{
	

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
	}
	
	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	// PREMESSA : BISOGNA CHIARIRE DA CHE PARTE FARLO RIMBALZARE AL MOMENTO DEL TASTO JUMP PREMUTO
	// FUNIONE INCOMPRETA NON 100% FUNZIONANTE.
	//** IL CONTROBALZO NON SI ATTIVA SEMPRE .... per qualche cazzo di motivo che adesso dopo 31827189 ore di lavoro non ce la faccio più poro dio  
	private void WallJumpingFunction()
	{
        if (WallSlidingKey)
        {
			
			IsTouchingFront = Physics2D.OverlapCircle(FrontCheck.position, k_FrontCheckRadius, m_WhatIsFront);
			
			if (WallJumpingKey)
			{
				Debug.Log("controbalzo");
				m_Rigidbody2D.velocity = new Vector2(xWallForce * -i_input, yWallForce);
				//WallJumpingKey = false;
			}
			if (IsTouchingFront == true && m_Grounded == false && i_input != 0)
			{
				
				WallSliding = true;
			}
			else
			{
			
				WallSliding = false;
			}
			if (WallSliding)
			{
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Clamp(m_Rigidbody2D.velocity.y, -WallSlidingSpeed, float.MaxValue));
				
			}

			/*if(Input.GetKeyDown(KeyCode.Space))
            {
				WallJumpingKey = true;
            }*/


		}
        
	}
}
