using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class FPScontroller : MonoBehaviour {

	// public vars
	public float mouseSensitivityX = 1;
	public float mouseSensitivityY = 1;
	public float walkSpeed = 6;
	public float jumpForce = 220;
	public LayerMask groundedMask;
	
	// System vars
	bool grounded;
	Vector3 moveAmount;
	Vector3 smoothMoveVelocity;
	float verticalLookRotation;
	public Transform cameraTransform;
	Rigidbody rigidBody;
	
	
	void Awake() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		//cameraTransform = Camera.main.transform;
		rigidBody = GetComponent<Rigidbody>();
	}
	
	void Update() {
		
		// Look rotation:
		//there is no limit to the rotation, rotating the player
		
		Vector3 rotationXaxis = (Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
		transform.Rotate(rotationXaxis);
		
		//gradually turned left, I do not know the reason. second attempt it worked as fine
		//rigidBody.MoveRotation(rigidBody.rotation * Quaternion.Euler(rotationXaxis));
		
		//there is limit to the rotation, rotating the camera
		verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation,-60,60);
		cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;
		
		// Calculate movement:
		float inputX = Input.GetAxisRaw("Horizontal");
		float inputY = Input.GetAxisRaw("Vertical");
		
		Vector3 moveDir = new Vector3(inputX,0, inputY).normalized;
		Vector3 targetMoveAmount = moveDir * walkSpeed;
		moveAmount = Vector3.SmoothDamp(moveAmount,targetMoveAmount,ref smoothMoveVelocity,.15f);
		
		// Jump
		if (Input.GetButtonDown("Jump")) {
			if (grounded) {
				rigidBody.AddForce(transform.up * jumpForce);
			}
		}
		
		// Grounded check
		Ray ray = new Ray(transform.position, -transform.up);
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit, rigidBody.transform.localScale.y + .1f, groundedMask)) {
			grounded = true;
		}
		else {
			grounded = false;
		}
		
	}
	
	void FixedUpdate() {
		// Apply movement to rigidbody
		Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
		//calculates the collider as well
		rigidBody.MovePosition(rigidBody.position + localMove);
	}
}
