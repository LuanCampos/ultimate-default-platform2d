using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[Header("Camera Movement:")]	
	[Tooltip("Character framing in relation to the screen center when facing right")]
	[SerializeField] private Vector3 offset;
	[Tooltip("Less that 1 to smoother movements, more than 1 to faster movements")]
	[SerializeField] private float smoothness = 1f;
	
	private Transform player;	
	private PlayerController playerController;
	
    void Start()
    {
		SetVariables();
		SetInitialPositioning();
    }

    void LateUpdate()
    {
		FollowThePlayer(offsetToPlayerFacingSide());
    }
	
	private void SetVariables()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		playerController = player.GetComponent<PlayerController>();
	}
	
	private void SetInitialPositioning()
	{
		transform.position = player.position + offset;
	}
	
	private void FollowThePlayer(Vector3 currentoffset)
	{
		transform.position = Vector3.Lerp(transform.position, PlayerPosition2D() + currentoffset, smoothness * Time.deltaTime);
	}
	
	private Vector3 offsetToPlayerFacingSide()
	{
		return playerController.IsPlayerFacingRight() ? offset : offsetFacingLeft();
	}
	
	private Vector3 offsetFacingLeft()
	{
		return new Vector3(-offset.x, offset.y, offset.z);
	}
	
	private Vector3 PlayerPosition2D()
	{
		return new Vector3(player.position.x, player.position.y, 0f);
	}
}