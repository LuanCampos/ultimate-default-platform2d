using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public class PlayerController : MonoBehaviour
{
	public float speed = 3f; 
	private float moveInput = 0f;
	private Rigidbody2D rigidBody;
	
	public float jumpForce = 6f;
	public int jumpMultiplier = 10; // number of frames the player can hold Jump button to get a higher jump
	public int numberOfJumps = 1;
	internal int jumpCount = 0;
	internal bool isGrounded = false;
	private int jumpMultipCount = 0;
	private bool jumpInput = false;
	
	[HideInInspector] public bool facingRight = true;
	private Animator anim; // using the variables Jump, Speed and ySpeed of the Animator
  
	void Start ()
	{
		rigidBody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}
  
	void Update ()
	{
		moveInput = Input.GetAxis ("Horizontal");
		
		if (Input.GetButtonDown("Jump"))
		{
			if (isGrounded)
			{
				jumpInput = true;
				jumpMultipCount = jumpMultiplier;
				anim.SetBool("Jump", true);
			}
			
			else if (jumpCount + 1 < numberOfJumps)
			{
				jumpInput = true;
				jumpMultipCount = jumpMultiplier;
				jumpCount += 1;
				anim.SetBool("Jump", true);
			}
		}
		
		if (Input.GetButtonUp("Jump"))
		{
			jumpInput = false;
			anim.SetBool("Jump", false);
		}
		
		anim.SetFloat("ySpeed", rigidBody.velocity.y);
		
	}
	
	void FixedUpdate()
	{
		if (moveInput != 0f)
		{
			rigidBody.velocity = new Vector2 (moveInput * speed, rigidBody.velocity.y);
			anim.SetFloat("Speed", Mathf.Abs(moveInput * speed));
		}
		
		else if (rigidBody.velocity.x != 0f)
		{
			rigidBody.velocity = new Vector2 (0f, rigidBody.velocity.y);
			anim.SetFloat("Speed", 0f);
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