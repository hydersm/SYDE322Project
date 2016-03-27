using UnityEngine;
using System.Collections;

public class SCCarSelfRighting : MonoBehaviour {

	public float selfRightingTorque = 1f;
	public float angleThreshold = 5;

	private Rigidbody body;

	void Start() {
		body = GetComponent<Rigidbody> ();
	}

	void FixedUpdate() {
		float angle = Vector3.Angle (transform.up, Vector3.up);
		if (angle > this.angleThreshold) {
			Vector3 axis = Vector3.Cross (transform.up, Vector3.up);
			body.AddTorque (axis * selfRightingTorque);
		}
	}
}
