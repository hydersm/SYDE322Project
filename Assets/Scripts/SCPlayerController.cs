using UnityEngine;
using System.Collections;

public class SCPlayerController : MonoBehaviour {

	public float deltaXScale = 1;
	public float deltaYScale = 1;
	public float delay = 0.1f;

	private Queue positions;
	public Vector3 newestPosition;

	// Use this for initialization
	void Start () {
		positions = new Queue ();
		newestPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float deltaX = Input.GetAxis ("Horizontal");
		float deltaY = Input.GetAxis ("Vertical");

		Vector3 delta = new Vector3 (deltaX*deltaXScale, 0, deltaY*deltaYScale);
		newestPosition += delta;

		SCPosition newScpos = new SCPosition (newestPosition, Time.time);

		positions.Enqueue (newScpos);

		while (positions.Count > 0) {
			if ((Time.time - this.delay) > ((SCPosition)positions.Peek ()).time) {
				SCPosition scpos = (SCPosition)positions.Dequeue ();
				transform.position = scpos.position;
			} else {
				break;
			}
		}
	}
}
