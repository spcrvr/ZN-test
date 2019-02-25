using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private bool _isGrounded = true;
    private float xMov;
	private float yMov;
    private float speed;
	private Vector3 moveDir;
	private Vector3 camForward;
	private Transform playerCam;
    private Rigidbody rb;
    private CharacterController cc;
	
	void Awake () {
        cc = GetComponent<CharacterController>();
		playerCam = GetComponentInChildren<Camera>().transform;
		moveDir = Vector3.zero;
		speed = 0.05f;
       // col.height = 2f;
       // col.center = new Vector3(0.15f, -0.51f, 0f);
       // col.radius = 0.5f;
        
	}
	
	void FixedUpdate () {
		HandleMovement();
	}

	private void HandleMovement()
	{
		xMov = Input.GetAxis("Horizontal");
		yMov = Input.GetAxis("Vertical");
		camForward = Vector3.Scale(playerCam.forward, new Vector3(1, 0, 1)).normalized;
		moveDir = (yMov * camForward + xMov * playerCam.right)*speed;
        Vector3 move = new Vector3(moveDir.x, 0, moveDir.z);
        cc.Move(move*Time.fixedDeltaTime*speed);
        //cc.Move(transform.position + moveDir);
        //if (moveDir != Vector3.zero)
        // {
        // turn player from old to new rotation by 0.15F with each update
        //   transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), 0.15F);
        // }
    }
}
