using UnityEngine;
using System.Collections;
using Photon;

public class SCNetworkCharacter : Photon.PunBehaviour {

	public Material playerMaterial;
	private Vector3 correctPlayerPos;

	// Use this for initialization
	void Start () {
		if (this.photonView.isMine) {
			this.GetComponent<PlayerController> ().enabled = true;
			this.GetComponent<Renderer>().material = playerMaterial;
		}
	}

	// Update is called once per frame
	void Update () {
		if (!this.photonView.isMine) {
			this.transform.position = Vector3.Lerp (this.transform.position, correctPlayerPos, Time.deltaTime * 5);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (transform.position);
		} else {
			this.correctPlayerPos = (Vector3)stream.ReceiveNext ();
		}
	}
}