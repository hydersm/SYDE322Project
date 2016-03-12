using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;

[System.Serializable]
public class AxleInfo {
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool motor;
	public bool steering;
}

public class SCCarController : Photon.PunBehaviour {
	public GameObject body;
	public GameObject m_camera;

	public Material playerMaterial;
	public List<AxleInfo> axleInfos; 
	public float maxMotorTorque;
	public float maxSteeringAngle;
	public float currentMotorTorque;
	public float currentSteeringAngle;
	public float downForce = 1;
	private Rigidbody rigidBody;

	public void Start() {
		if (photonView.isMine) {
			Material[] playerMaterials = { playerMaterial, playerMaterial };
			body.GetComponent<Renderer> ().materials = playerMaterials;
		} else {
			Destroy (m_camera);
		}

		rigidBody = GetComponent<Rigidbody> ();
	}

	public void FixedUpdate()
	{
		if (photonView.isMine) {
			currentMotorTorque = maxMotorTorque * Input.GetAxis ("Vertical");
			currentSteeringAngle = maxSteeringAngle * Input.GetAxis ("Horizontal");
		}

		foreach (AxleInfo axleInfo in axleInfos) {
			if (axleInfo.steering) {
				axleInfo.leftWheel.steerAngle = currentSteeringAngle;
				axleInfo.rightWheel.steerAngle = currentSteeringAngle;
			}
			if (axleInfo.motor) {
				axleInfo.leftWheel.motorTorque = currentMotorTorque;
				axleInfo.rightWheel.motorTorque = currentMotorTorque;
			}
			ApplyLocalPositionToVisuals(axleInfo.leftWheel);
			ApplyLocalPositionToVisuals(axleInfo.rightWheel);
		}

		if (Input.GetButtonDown("Flip") && photonView.isMine){
			rigidBody.angularVelocity = Vector3.zero;
			rigidBody.velocity = Vector3.zero;
			transform.position += Vector3.up;
			transform.LookAt(transform.position + transform.TransformDirection(Vector3.forward), Vector3.up);
		}

		AddDownForce ();
	}

	// finds the corresponding visual wheel
	// correctly applies the transform
	public void ApplyLocalPositionToVisuals(WheelCollider collider)
	{
		if (collider.transform.childCount == 0) {
			return;
		}

		Transform visualWheel = collider.transform.GetChild(0);

		Vector3 position;
		Quaternion rotation;
		collider.GetWorldPose(out position, out rotation);

		visualWheel.transform.position = position;
		visualWheel.transform.rotation = rotation;
	}

	//to add more grip as speed increases
	public void AddDownForce() {
		rigidBody.AddForce (-transform.up * downForce * rigidBody.velocity.magnitude);
	}
}