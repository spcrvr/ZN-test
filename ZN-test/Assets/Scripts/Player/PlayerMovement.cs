using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	private float xMov;
	private float yMov;
	private Vector3 moveDir;
	private Vector3 camForward;
	private Transform playerCam;
	private Rigidbody rb;
	private CapsuleCollider col;
	private float speed;
	void Awake () {
		rb = GetComponent<Rigidbody>();
		col = GetComponent<CapsuleCollider>();
		playerCam = GetComponentInChildren<Camera>().transform;
		moveDir = Vector3.zero;
		speed = 0.05f;
		col.height = 2f;
        col.center = new Vector3(0.15f, -0.51f, 0f);
        col.radius = 0.5f;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		HandleMovement();
	}

	private void HandleMovement()
	{
		xMov = Input.GetAxis("Horizontal");
		yMov = Input.GetAxis("Vertical");
		camForward = Vector3.Scale(playerCam.forward, new Vector3(1, 0, 1)).normalized;
		moveDir = (yMov * camForward + xMov * playerCam.right)*speed;
		rb.MovePosition(transform.position + moveDir);
        //if (moveDir != Vector3.zero)
       // {
            // turn player from old to new rotation by 0.15F with each update
         //   transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), 0.15F);
       // }
	}
}
