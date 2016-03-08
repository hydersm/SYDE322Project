using UnityEngine;
using System.Collections;

public class SCPosition : Object {

	public Vector3 position;
	public float time;

	public SCPosition(Vector3 position, float time) {
		this.position = position;
		this.time = time;
	}
}
