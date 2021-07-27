using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[Header("[Camera Movement]")]	
	[Tooltip("Character framing in relation to the screen center when facing right")]
	[SerializeField] private Vector3 offSet;
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
		FollowThePlayer(OffsetToPlayerFacingSide());
    }
	
	private void SetVariables()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		playerController = player.GetComponent<PlayerController>();
	}
	
	private void SetInitialPositioning()
	{
		transform.position = player.position + offSet;
	}
	
	private void FollowThePlayer(Vector3 currentOffset)
	{
		transform.position = Vector3.Lerp(transform.position, PlayerPosition2D() + currentOffset, smoothness * Time.deltaTime);
	}
	
	private Vector3 OffsetToPlayerFacingSide()
	{
		return playerController.IsPlayerFacingRight() ? offSet : OffsetFacingLeft();
	}
	
	private Vector3 OffsetFacingLeft()
	{
		return new Vector3(-offSet.x, offSet.y, offSet.z);
	}
	
	private Vector3 PlayerPosition2D()
	{
		return new Vector3(player.position.x, player.position.y, 0f);
	}
}