using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Vector3 OffSet;
	public float smooth = 1f; // less that 1 to smoother movements, more than 1 to faster movements
	public Transform target;
	private PlayerController playerController;
	
    void Start()
    {
		playerController = target.GetComponent<PlayerController>();
        transform.position = target.position + OffSet;
    }

    void LateUpdate()
    {
		if (playerController.facingRight)
		{
			transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, 0f) + OffSet, smooth * Time.deltaTime);
		}
		
		else
		{
			transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, 0f) + new Vector3(-OffSet.x, OffSet.y, OffSet.z), smooth * Time.deltaTime);
		}
    }
	
}