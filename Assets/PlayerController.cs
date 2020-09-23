using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
	// movement variables
	[Tooltip("The speed of the character")]
	public float speed = 4f; 
	
	private float moveInput = 0f;
	private Rigidbody2D rigidBody;
	
	// jump variables
	[Tooltip("The force of jump of the character")]
	public float jumpForce = 7f;
	[Tooltip("How many frames the player can hold Jump Button to get a higher jump")]
	public int jumpBoost = 10;
	[Tooltip("How many frames before the character is grounded the player can press Jump Button and still jump")]
	public int earlyInputMargin = 8;
	[Tooltip("How many frames after the character was grounded the player can press Jump Button and still jump")]
	public int coyoteTime = 5;
	[Tooltip("How many times the character can jump after touching the ground once")]
	public int numberOfJumps = 1;

	internal bool isGrounded = false;
	internal int jumpCount = 0;
	internal int coyoteTimeCount = 0;
	
	private bool jumpInput = false;
	private int earlyInputCount = 0;
	private int jumpBoostCount = 0;
	
	[HideInInspector]
	public bool facingRight = true;
  
	void Start ()
	{
		rigidBody = GetComponent<Rigidbody2D> ();
	}

	void Update ()
	{
		// get movement input
		moveInput = Input.GetAxis ("Horizontal");
		
		// get jump input
		if (Input.GetButtonDown("Jump") || earlyInputCount > 0)
		{
			if (isGrounded || coyoteTimeCount > 0)
			{
				jumpInput = true;
				jumpBoostCount = jumpBoost;
			}
			
			else if (jumpCount + 1 < numberOfJumps)
			{
				jumpInput = true;
				jumpBoostCount = jumpBoost;
				jumpCount += 1;
			}
			
			else if (earlyInputCount == 0)
			{
				earlyInputCount = earlyInputMargin;
			}
		}
		
		if (Input.GetButtonUp("Jump"))
		{
			jumpInput = false;
		}
		
	}
	
	void FixedUpdate()
	{
		// manage movement
		if (moveInput != 0f)
		{
			rigidBody.velocity = new Vector2 (moveInput * speed, rigidBody.velocity.y);
		}
		
		else if (rigidBody.velocity.x != 0f)
		{
			rigidBody.velocity = new Vector2 (0f, rigidBody.velocity.y);
		}
		
		// manage jump
		if (jumpInput && jumpBoostCount > 0)
		{
			rigidBody.velocity = new Vector2 (rigidBody.velocity.x, jumpForce);
			jumpBoostCount -= 1;
		}
		
		if (coyoteTimeCount > 0)
		{
			coyoteTimeCount -= 1;
		}
		
		if (earlyInputCount > 0)
		{
			earlyInputCount -= 1;
		}
		
		// manage what side the character is facing
		if ((rigidBody.velocity.x > 0f && !facingRight) || (rigidBody.velocity.x < 0f && facingRight))
		{
			transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1,1,1));
			facingRight = !facingRight;
		}
	
	}
	
}