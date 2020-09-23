using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
	public float speed = 3f; 
	private float moveInput = 0f;
	private Rigidbody2D rigidBody;
	
	public float jumpForce = 6f;
	public int jumpMultiplier = 10;
	public int jumpInputTime = 20;
	public int numberOfJumps = 1;
	internal int jumpCount = 0;
	internal bool isGrounded = false;
	private int jumpMultipCount = 0;
	private int jumpInputCount = 0;
	private bool jumpInput = false;
	
	[HideInInspector] public bool facingRight = true;
  
	void Start ()
	{
		rigidBody = GetComponent<Rigidbody2D> ();
	}

	void Update ()
	{
		moveInput = Input.GetAxis ("Horizontal");
		
		if (Input.GetButtonDown("Jump"))
		{
			jumpInputCount = jumpInputTime;
		}
		
		if (jumpInputCount > 0f)
		{
			if (isGrounded)
			{
				jumpInput = true;
				jumpMultipCount = jumpMultiplier;
			}
			
			else if (jumpCount + 1 < numberOfJumps)
			{
				jumpInput = true;
				jumpMultipCount = jumpMultiplier;
				jumpCount += 1;
			}
			
		}
		
		if (Input.GetButtonUp("Jump"))
		{
			jumpInput = false;
		}
		
	}
	
	void FixedUpdate()
	{
		if (moveInput != 0f)
		{
			rigidBody.velocity = new Vector2 (moveInput * speed, rigidBody.velocity.y);
		}
		
		else if (rigidBody.velocity.x != 0f)
		{
			rigidBody.velocity = new Vector2 (0f, rigidBody.velocity.y);
		}
		
		if (jumpInputCount > 0f)
		{
			jumpInputCount -= 1;
		}
		
		if (jumpInput && jumpMultipCount > 0)
		{
			rigidBody.velocity = new Vector2 (rigidBody.velocity.x, jumpForce);
			jumpMultipCount -= 1;
		}
		
		if ((rigidBody.velocity.x > 0f && !facingRight) || (rigidBody.velocity.x < 0f && facingRight))
		{
			transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1,1,1));
			facingRight = !facingRight;
		}
	
	}
	
}