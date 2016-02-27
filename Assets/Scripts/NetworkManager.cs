using UnityEngine;
using System.Collections;
using Photon;

public class NetworkManager : Photon.PunBehaviour {

	public string gameVersion = "1.0";

	// Use this for initialization
	void Start () {
		//PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings (this.gameVersion);
	}

	// Update is called once per frame
	void Update () {
	}

	public override void OnJoinedLobby ()
	{
		base.OnJoinedLobby ();
		PhotonNetwork.JoinRandomRoom ();
	}

	public void OnPhotonRandomJoinFailed() {
		Debug.Log ("Random room join failed! Creating a new room.");
		PhotonNetwork.CreateRoom (null);
	}

	public override void OnJoinedRoom() {
		PhotonNetwork.Instantiate ("SCPlayer", new Vector3(0f, 0.5f, 0f), Quaternion.identity, 0);
	}
}
