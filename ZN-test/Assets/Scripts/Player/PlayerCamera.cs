using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
	private Vector2 lookDir;
	private Vector2 smoothVal;
	private Vector2 mouseLook;
	private float sensitivity;
	private float smoothing;
	private Transform playerCube;

	void Awake () {
		sensitivity = 2.0f;
		smoothing = 1.0f;
		playerCube = this.transform.parent;
	}

	void Start () { Cursor.lockState = CursorLockMode.Locked; }
	
	void FixedUpdate () {
		HandleLooking();
	}

	void HandleLooking()
	{
		lookDir = new Vector2(Input.GetAxisRaw("Mouse X"),Input.GetAxisRaw("Mouse Y"));
		lookDir = Vector2.Scale(lookDir, new Vector2(sensitivity*smoothing,sensitivity*smoothing));
		smoothVal = new Vector2(
			Mathf.Lerp(smoothVal.x, lookDir.x, 1f/smoothing),
			Mathf.Lerp(smoothVal.y, lookDir.y, 1f/smoothing)
		);
		mouseLook += smoothVal;
		mouseLook.y = Mathf.Clamp(mouseLook.y,-80f,80f);
		transform.localRotation = Quaternion.AngleAxis(-mouseLook.y,Vector3.right);
		playerCube.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, playerCube.transform.up);
	}
}
