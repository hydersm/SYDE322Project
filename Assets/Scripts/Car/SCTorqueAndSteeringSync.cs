using UnityEngine;
using System.Collections;
using Photon;

public class SCTorqueAndSteeringSync : Photon.PunBehaviour {

	SCCarController carController;
	// Use this for initialization
	void Start () {
		carController = GetComponent<SCCarController> ();
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (carController.currentMotorTorque);
			stream.SendNext (carController.currentSteeringAngle);
		} else {
			carController.currentMotorTorque = (float)stream.ReceiveNext ();
			carController.currentSteeringAngle = (float)stream.ReceiveNext ();
		}
	}
}