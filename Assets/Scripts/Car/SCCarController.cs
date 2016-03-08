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

	public void Start() {
		if (photonView.isMine) {
			body.GetComponent<Renderer> ().material = playerMaterial;
		} else {
			Destroy (m_camera);
		}
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
}