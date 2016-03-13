using UnityEngine;
using System.Collections;
using Photon;

public class SCExterpSync : Photon.PunBehaviour {

	private SCCarController carController;
	private Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
		carController = GetComponent<SCCarController> ();
		rigidBody = GetComponent<Rigidbody> ();
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (carController != null) {
			if (stream.isWriting) {
				stream.SendNext (carController.currentMotorTorque);
				stream.SendNext (carController.currentSteeringAngle);
				stream.SendNext (rigidBody.velocity);
			} else {
				carController.currentMotorTorque = (float)stream.ReceiveNext ();
				carController.currentSteeringAngle = (float)stream.ReceiveNext ();
				rigidBody.velocity = (Vector3)stream.ReceiveNext ();
			}
		}
	}
}