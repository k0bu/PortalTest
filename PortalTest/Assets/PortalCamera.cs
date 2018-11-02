using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour {

	public Transform playerCamera;
	public Transform portal;
	public Transform otherPortal;

	void Update(){
		Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;
		//print(playerOffsetFromPortal);
		transform.position = portal.position + playerOffsetFromPortal;

		//Returns the angle in degrees between two rotations a and b.
		float angularDifferenceBetweenPortalRotation = Quaternion.Angle(portal.rotation, otherPortal.rotation);
		print(angularDifferenceBetweenPortalRotation);
		//Creates a rotation which rotates "angle" degrees around axis.
		Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotation, Vector3.up);
		print(portalRotationalDifference);
		Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
		transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
	}

}
