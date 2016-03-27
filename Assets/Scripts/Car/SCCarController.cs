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

	public List<AxleInfo> axleInfos; 
	public float maxMotorTorque;
	public float maxBrakeTorque;
	public float reverseTorque;
	public float brakeConstant = 2f;
	public float maxSteeringAngle;
	public float currentMotorTorque;
	public float currentBrakeTorque;
	public float currentSteeringAngle;
	public float downForce = 1;
	public float frictionStiffness = 0.5f;
	public float steerHelper = 0.5f;

	private Rigidbody rigidBody;
	private float oldRotation;

	public void Start() {
		rigidBody = GetComponent<Rigidbody> ();
	}

	public void FixedUpdate()
	{
		if (photonView.isMine) {
			float verticalAxis = Input.GetAxis ("Vertical");
			if (verticalAxis < 0) {
				if (Vector3.Dot (transform.InverseTransformVector (this.rigidBody.velocity), Vector3.forward) > 0) {
					currentMotorTorque = 0;
					currentBrakeTorque = -verticalAxis * maxBrakeTorque;
				} else {
					currentMotorTorque = verticalAxis * reverseTorque;
					currentBrakeTorque = 0;
				}
			} else {
				if (Vector3.Dot (transform.InverseTransformVector (this.rigidBody.velocity), Vector3.forward) > 0) {
					currentMotorTorque = maxMotorTorque * verticalAxis;
					currentBrakeTorque = 0;
				} else {
					currentMotorTorque = 0;
					currentBrakeTorque = verticalAxis * maxBrakeTorque;
				}
			}

//			currentMotorTorque = maxMotorTorque * Input.GetAxis ("Vertical");
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

				axleInfo.leftWheel.brakeTorque = currentBrakeTorque;
				axleInfo.rightWheel.brakeTorque = currentBrakeTorque;
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
		SetWheelFrictionProperties ();
		SteerHelper ();
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
		foreach (AxleInfo axleInfo in axleInfos) {
			WheelHit wheelhit;
			axleInfo.leftWheel.GetGroundHit (out wheelhit);
			if (wheelhit.normal == Vector3.zero) {
				return;
			}

			axleInfo.rightWheel.GetGroundHit (out wheelhit);
			if (wheelhit.normal == Vector3.zero) {
				return;
			}
		}

		rigidBody.AddForce (-transform.up * downForce * rigidBody.velocity.magnitude);
	}

	public void SetWheelFrictionProperties() {
		foreach (AxleInfo axleInfo in axleInfos) {
			WheelFrictionCurve f = axleInfo.leftWheel.forwardFriction;
			f.stiffness = frictionStiffness;
			axleInfo.leftWheel.forwardFriction = f;
			axleInfo.rightWheel.forwardFriction = f;

			WheelFrictionCurve s = axleInfo.leftWheel.sidewaysFriction;
			s.stiffness = frictionStiffness;
			axleInfo.leftWheel.sidewaysFriction = s;
			axleInfo.rightWheel.sidewaysFriction = s;
		}
	}

	public void SteerHelper() {
		foreach (AxleInfo axleInfo in axleInfos) {
			WheelHit wheelhit;
			axleInfo.leftWheel.GetGroundHit (out wheelhit);
			if (wheelhit.normal == Vector3.zero) {
				return;
			}

			axleInfo.rightWheel.GetGroundHit (out wheelhit);
			if (wheelhit.normal == Vector3.zero) {
				return;
			}
		}

		if (Mathf.Abs(oldRotation - transform.eulerAngles.y) < 10f)
		{
			var turnadjust = (transform.eulerAngles.y - oldRotation) * steerHelper;
			Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
			rigidBody.velocity = velRotation * rigidBody.velocity;
		}
		oldRotation = transform.eulerAngles.y;
	}
}