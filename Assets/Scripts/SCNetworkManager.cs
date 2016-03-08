﻿using UnityEngine;
using System.Collections;
using Photon;

public class SCNetworkManager : Photon.PunBehaviour {

	public string gameVersion = "1.0";

	// Use this for initialization
	void Start () {
		//PhotonNetwork.logLevel = PhotonLogLevel.Full;
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
		PhotonNetwork.Instantiate ("SCPlayer", new Vector3(0f, 0.5f, 0f), Quaternion.identity, 0);
	}

	public void OnDisable() {
		Debug.Log ("Disconnecting from Photon.");
		PhotonNetwork.Disconnect ();
	}
}