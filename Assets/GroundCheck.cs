using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class GroundCheck : MonoBehaviour
{
	private Animator anim;
	private PlayerController playerController;
	
    void Start()
    {
		anim = gameObject.transform.parent.gameObject.GetComponent<Animator>();
		playerController = gameObject.transform.parent.gameObject.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D col)
	{
		playerController.jumpCount = 0;
		playerController.isGrounded = true;
		anim.SetBool("isGrounded", true);
	}
	
	private void OnTriggerExit2D(Collider2D col)
	{
		playerController.isGrounded = false;
		anim.SetBool("isGrounded", false);
	}
	
}