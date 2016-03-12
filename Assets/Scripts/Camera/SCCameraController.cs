using UnityEngine;
using System.Collections;

public class SCCameraController : MonoBehaviour {

	public GameObject cameraLookTarget;
	public GameObject cameraPositionTarget;
	public float positionLerpFactor = 1;
	public float rotationLerpFactor = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		transform.position = Vector3.Lerp (transform.position, cameraPositionTarget.transform.position, Time.deltaTime * positionLerpFactor);
//		transform.position = cameraLookTarget.transform.position;
		//transform.rotation = Quaternion.Lerp (transform.rotation, cameraTarget.transform.rotation, Time.deltaTime * rotationLerpFactor);
//		Vector3 relativePos = cameraLookTarget.transform.position - transform.position;
//		transform.rotation = Quaternion.LookRotation(relativePos);
	}
}
