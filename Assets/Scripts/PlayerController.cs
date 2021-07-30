using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
	[Header("[Character Movement]")]
	[Tooltip("The speed of the character")]
	[SerializeField] private float speed = 4f; 
	[Tooltip("The default jump force of the character")]
	[SerializeField] private float jumpForce = 7f;
	[Tooltip("How many frames the player can hold Jump Button to get a higher jump")]
	[SerializeField] private int jumpBoost = 10;
	[Tooltip("How many frames before touch the grounded the player can press Jump Button and still jump")]
	[SerializeField] private int earlyInputMargin = 8;
	[Tooltip("How many frames after the character leave the grounded the player still can jump")]
	[SerializeField] private int coyoteTime = 5;
	[Tooltip("How many times the character can jump after touching the ground once")]
	[SerializeField] private int numberOfJumps = 1;
	
	private bool isGrounded, facingRight, jumpInput;
	private int earlyInputCounter, coyoteTimeCounter;
	private int jumpCounter, jumpBoostCounter;
	private float moveInput;
	private Rigidbody2D rigidBody;
	private Vector3 inverseXAxis = new Vector3(-1,1,1);
	
	void Start()
	{
		SetVariables();
	}

	void Update()
	{
		GetMovementInput();
		GetJumpInput();		
	}
	
	void FixedUpdate()
	{
		Move();
		Jump();
		FaceTheGoingSide();
	}
	
	public bool IsPlayerFacingRight()
	{
		return facingRight;
	}
	
	public void TouchTheGround()
	{
		isGrounded = true;
		jumpCounter = 0;
		coyoteTimeCounter = 0;
	}
	
	public void LeaveTheGround()
	{
		isGrounded = false;
		coyoteTimeCounter = coyoteTime;
	}
	
	private void SetVariables()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		facingRight = true;
	}
	
	private void GetMovementInput()
	{
		moveInput = Input.GetAxis("Horizontal");
	}
	
	private void GetJumpInput()
	{
		GetJumpInputPress();
		GetJumpInputRelease();
	}
	
	private void GetJumpInputPress()
	{
		if (HasJumpInput())
		{
			TriggerJumpInput();
		}
	}
	
	private bool HasJumpInput()
	{
		return Input.GetButtonDown("Jump") || earlyInputCounter > 0;
	}
	
	private void TriggerJumpInput()
	{
		if (CanJumpThisFrame())
			TriggerJump();
		
		else if (EarlyInputNotTriggerYet())
			TriggerEarlyInput();
	}
	
	private bool CanJumpThisFrame()
	{
		return IsConsideredGrounded() || HasMoreJumpsToUse();
	}
	
	private bool IsConsideredGrounded()
	{
		return isGrounded || coyoteTimeCounter > 0;
	}
	
	private bool HasMoreJumpsToUse()
	{
		return jumpCounter + 1 < numberOfJumps;
	}
	
	private void TriggerJump()
	{
		jumpInput = true;
		jumpBoostCounter = jumpBoost;
		
		if (!IsConsideredGrounded())
			IncreaseJumpCount();
	}
	
	private void IncreaseJumpCount()
	{
		jumpCounter += 1;
	}
	
	private bool EarlyInputNotTriggerYet()
	{
		return earlyInputCounter == 0;
	}
	
	private void TriggerEarlyInput()
	{
		earlyInputCounter = earlyInputMargin;
	}
	
	private void GetJumpInputRelease()
	{
		if (Input.GetButtonUp("Jump"))
			jumpInput = false;
	}
	
	private void Move()
	{
		if (HasMovementInput())
			UpdateMovementVelocity();
		
		else if (HasMovementVelocity())
			StopMovementVelocity();
	}
	
	private bool HasMovementInput()
	{
		return moveInput != 0f;
	}
	
	private void UpdateMovementVelocity()
	{
		rigidBody.velocity = new Vector2 (moveInput * speed, rigidBody.velocity.y);
	}
	
	private bool HasMovementVelocity()
	{
		return rigidBody.velocity.x != 0f;
	}
	
	private void StopMovementVelocity()
	{
		rigidBody.velocity = new Vector2 (0f, rigidBody.velocity.y);
	}
	
	private void Jump()
	{
		if (jumpInput && HasJumpBoost())
		{
			IncreaseJumpVelocity();
			UpdateJumpBoostCount();
		}
		
		UpdateCoyoteTimeCount();
		UpdateEarlyInputCount();
	}
	
	private bool HasJumpBoost()
	{
		return jumpBoostCounter > 0;
	}
	
	private void IncreaseJumpVelocity()
	{
		rigidBody.velocity = new Vector2 (rigidBody.velocity.x, jumpForce);
	}
	
	private void UpdateJumpBoostCount()
	{
		jumpBoostCounter -= 1;
	}
	
	private void UpdateCoyoteTimeCount()
	{
		if (coyoteTimeCounter > 0)
			coyoteTimeCounter -= 1;
	}
	
	private void UpdateEarlyInputCount()
	{
		if (earlyInputCounter > 0)
			earlyInputCounter -= 1;
	}	
	
	private void FaceTheGoingSide()
	{
		if ((GoingLeft() && facingRight) || (GoingRight() && !facingRight))
			ChangeTheFacingSide();
	}
	
	private bool GoingLeft()
	{
		return rigidBody.velocity.x < 0f;
	}
	
	private bool GoingRight()
	{
		return rigidBody.velocity.x > 0f;
	}
	
	private void ChangeTheFacingSide()
	{
		transform.localScale = Vector3.Scale(transform.localScale, inverseXAxis);
		facingRight = !facingRight;
	}
}