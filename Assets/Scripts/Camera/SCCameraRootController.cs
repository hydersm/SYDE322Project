using UnityEngine;
using System.Collections;

public class SCCameraRootController : MonoBehaviour {

	[HideInInspector]
	public Transform target;

	public float rotationLerpFactor;
	public float maxAngle = 30;
	public static SCCameraRootController firstInstance;

	// Use this for initialization
	void Start () {
		if (SCCameraRootController.firstInstance == null) {
			SCCameraRootController.firstInstance = this;
		} else {
			Destroy (gameObject);
		}

		target = null;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (target != null) {
			transform.position = target.position;

			float angle = Vector3.Angle (transform.forward, target.forward);
//			if (angle > maxAngle) {
//				Vector3 cross = Vector3.Cross (transform.forward, target.transform.forward);
//				Vector3 newLook = Quaternion.AngleAxis (-maxAngle, cross) * target.transform.forward;
//				transform.forward = newLook;
//			}

			transform.rotation = Quaternion.Lerp (transform.rotation, target.rotation, Time.deltaTime * rotationLerpFactor);
		}

	}
}
