using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SCCarCollision : MonoBehaviour {

	public float forceFactor = 1f;
	public float upOffset = 0f;
	private Rigidbody body;

	void Start() {
		body = GetComponent<Rigidbody> ();
		Physics.IgnoreLayerCollision (8, 8);
		Physics.IgnoreLayerCollision (8, 9);
	}

	void OnCollisionEnter(Collision collisionInfo) {

		Dictionary<int, Vector3> collisionSums = new Dictionary<int, Vector3> ();
		Dictionary<int, Collider> colliderDict = new Dictionary<int, Collider> ();
		Dictionary<int, int> count = new Dictionary<int, int> ();

		foreach (ContactPoint contact in collisionInfo.contacts) {
			int id = contact.otherCollider.gameObject.GetInstanceID ();

			if (contact.thisCollider.gameObject.tag == "BodyFront" && contact.otherCollider.gameObject.tag == "BodyRest") {
				if (collisionSums.ContainsKey (id)) {
					collisionSums [id] += contact.point;
					count [id]++;
				} else {
					collisionSums [id] = contact.point;
					count [id] = 1;
					colliderDict [id] = contact.otherCollider;
				}
			}

		}

		foreach (KeyValuePair<int, Vector3> entry in collisionSums) {
			Vector3 avgPoint = entry.Value / (float)count [entry.Key];
			Vector3 forceDir = avgPoint - transform.position + upOffset * Vector3.up;

			Vector3 midPoint = 0.5f*((BoxCollider)colliderDict [entry.Key]).center + 0.5f*avgPoint;
			colliderDict [entry.Key].attachedRigidbody.AddForce (forceFactor * body.velocity.magnitude * forceDir);
		}
	}

}
