using UnityEngine;
using System.Collections;

public class SCRotateController : MonoBehaviour {

	public float angularVelcity = 01;
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Rotate (new Vector3 (0, Time.deltaTime * angularVelcity, 0));
	}
}
