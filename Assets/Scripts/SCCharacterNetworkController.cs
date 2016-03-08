using UnityEngine;
using System.Collections;
using Photon;

public class SCCharacterNetworkController : Photon.PunBehaviour {

	public Material playerMaterial;

	private Vector3 correctPlayerPos;
	private SCPlayerController playerController;

	// Use this for initialization
	void Start () {
		if (this.photonView.isMine) {
			playerController = GetComponent<SCPlayerController> ();
			playerController.enabled = true;
			GetComponent<Renderer>().material = playerMaterial;
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
			if (playerController != null) {
				stream.SendNext (playerController.newestPosition);
				playerController.delay = (PhotonNetwork.GetPing () * 2.0f) / 1000.0f;
			}
		} else {
			this.correctPlayerPos = (Vector3)stream.ReceiveNext ();
		}
	}
}