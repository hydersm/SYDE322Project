using UnityEngine;
using System.Collections;
using Photon;

public class SCInterpSync : Photon.PunBehaviour {

	Vector3 targetPos;
	Quaternion targetRot;

	public void Update() {
		if (!photonView.isMine) {
			transform.position = Vector3.Lerp (transform.position, targetPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp (transform.rotation, targetRot, Time.deltaTime * 5);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {

		if (stream.isWriting) {
			if (transform != null) {
				stream.SendNext (transform.position);
				stream.SendNext (transform.rotation);
			}
		} else {
			targetPos = (Vector3)stream.ReceiveNext ();
			targetRot = (Quaternion)stream.ReceiveNext ();
		}
	}

}
