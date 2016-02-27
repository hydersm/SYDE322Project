using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float deltaXScale = 1;
	public float deltaYScale = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float deltaX = Input.GetAxis ("Horizontal");
		float deltaY = Input.GetAxis ("Vertical");

		transform.Translate (deltaXScale * deltaX * Time.deltaTime * Vector3.right);
		transform.Translate (deltaYScale * deltaY * Time.deltaTime * Vector3.forward);
	}
}
