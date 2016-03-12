using UnityEngine;
using System.Collections;
using Photon;

public class SCNetworkManager : Photon.PunBehaviour {

	public string gameVersion = "1.0";
	public int sendRate = 30;
	public SCCameraRootController mainCameraRootController;

	// Use this for initialization
	void Start () {
		//PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.sendRate = sendRate;
		PhotonNetwork.sendRateOnSerialize = sendRate;
		PhotonNetwork.ConnectUsingSettings (this.gameVersion);
	}

	// Update is called once per frame
	void Update () {
	}

	public override void OnConnectedToMaster() {
		Debug.Log ("Connected to Master.\n Joining Default Lobby.");
		base.OnConnectedToMaster ();
		PhotonNetwork.JoinLobby();
	}

	public override void OnJoinedLobby ()
	{
		Debug.Log ("Joined Lobby.\n Joining Random Room.");
		base.OnJoinedLobby ();
		PhotonNetwork.JoinRandomRoom ();
	}

	public void OnPhotonRandomJoinFailed() {
		Debug.Log ("Random room join failed! Creating a new room.");
		PhotonNetwork.CreateRoom (null);
	}

	public override void OnJoinedRoom() {

		if (PhotonNetwork.playerList.Length > 6) {
			PhotonNetwork.Disconnect ();
		}

		int randomColor = 0;
		System.Random rnd = new System.Random ();

		while (true) {
			randomColor = rnd.Next (0, 6);
			bool found = false;

			foreach (PhotonPlayer player in PhotonNetwork.playerList) {
				if (player != PhotonNetwork.player) {
					found = (int)player.customProperties ["color"] == randomColor;	
				}
			}

			if (!found) {
				Debug.Log (string.Format("Found color: {0}", randomColor));
				break;
			}
		}

		ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable ();
		hash ["color"] = randomColor;
		PhotonNetwork.player.SetCustomProperties (hash);

		Vector2 randPos = Random.insideUnitCircle * 50;
		Vector3 spawnPos = new Vector3 (randPos.x, 1f, randPos.y);
		GameObject newCar = PhotonNetwork.Instantiate ("SCCar3", spawnPos, Quaternion.identity, 0);

	}

	public void OnDisable() {
		Debug.Log ("Disconnecting from Photon.");
		PhotonNetwork.Disconnect ();
	}
}
