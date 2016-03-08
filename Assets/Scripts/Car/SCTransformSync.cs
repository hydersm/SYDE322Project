using UnityEngine;
using System.Collections;
using Photon;

public class SCTransformSync : Photon.PunBehaviour {

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			if (transform != null) {
				stream.SendNext (transform.position);
				stream.SendNext (transform.rotation);
			}
		} else {
			Vector3 pos = (Vector3)stream.ReceiveNext ();
			Quaternion rot = (Quaternion)stream.ReceiveNext ();

			if ((transform.position - pos).magnitude > 9) {
				transform.position = pos;
				transform.rotation = rot;
			}
		}
	}

}
