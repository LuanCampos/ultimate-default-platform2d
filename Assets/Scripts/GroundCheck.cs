using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class GroundCheck : MonoBehaviour
{
	private PlayerController playerController;
	
    void Start()
    {
		SetVariables();
    }
	
	private void SetVariables()
	{
		playerController = gameObject.transform.parent.gameObject.GetComponent<PlayerController>();
	}

    private void OnTriggerEnter2D(Collider2D col)
	{
		playerController.TouchTheGround();
	}
	
	private void OnTriggerExit2D(Collider2D col)
	{
		playerController.LeaveTheGround();
	}
	
}