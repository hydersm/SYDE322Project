using UnityEngine;
using System.Collections;
using Photon;
using System;

public class SCNetworkManager : Photon.PunBehaviour {

	public static SCNetworkManager instance;

	public string gameVersion = "1.0";
	public int sendRate = 30;
	public SCCameraRootController mainCameraRootController;
	public bool isConnectedToMaster;
	public SCAccount account;
	public SCCar car;
	public bool canControlCar = false;

	private bool createGame;
	private string gameName;
	private int numPlayers;
	private Action<RoomInfo[]> roomListCallback;

	public void Awake() {
		if (instance == null) {
			DontDestroyOnLoad (gameObject);
			instance = this;
			isConnectedToMaster = false;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

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
		Debug.Log ("Connected to Master.");
		base.OnConnectedToMaster ();
		isConnectedToMaster = true;
	}

	public void JoinDefaultLobbyAndCreateGame(string name, int numberOfPlayers) {
		createGame = true;
		gameName = name;
		this.numPlayers = numberOfPlayers;
		PhotonNetwork.JoinLobby ();
	}

	public void JoinDefaultLobbyAndGetRoomList(Action<RoomInfo[]> callback) {
		createGame = false;
		roomListCallback = callback;
		PhotonNetwork.JoinLobby ();
	}

	public override void OnJoinedLobby ()
	{
		Debug.Log ("Joined Lobby.\n Joining Random Room.");
		base.OnJoinedLobby ();

		if (createGame) {
			RoomOptions roomOptions = new RoomOptions ();
			roomOptions.isVisible = true;
			roomOptions.isOpen = true;
			roomOptions.cleanupCacheOnLeave = true;
			roomOptions.maxPlayers = (byte) numPlayers;

			SCSceneController.instance.LoadLevel ("MainScene");
			PhotonNetwork.CreateRoom (gameName, roomOptions, TypedLobby.Default);
//			PhotonNetwork.CreateRoom(gameName);
		}
	}

	public override void OnReceivedRoomListUpdate() {
		Debug.Log ("Photon Room Update Called!");
		RoomInfo[] roomInfos = PhotonNetwork.GetRoomList ();
		if (roomListCallback != null) {
			roomListCallback (roomInfos);
		}
	}

	public void JoinRoom(string roomName) {
		SCSceneController.instance.LoadLevel ("MainScene");
		PhotonNetwork.JoinRoom (roomName);
	}

	public void OnPhotonRandomJoinFailed() {
//		Debug.Log ("Random room join failed! Creating a new room.");
//		PhotonNetwork.CreateRoom (null);
	}

	public override void OnJoinedRoom() {

		canControlCar = false;

		ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable ();
		hash ["color"] = car.color;
		hash ["name"] = account.name;
		PhotonNetwork.player.SetCustomProperties (hash);

		Vector2 randPos = UnityEngine.Random.insideUnitCircle * 50;
		Vector3 spawnPos = new Vector3 (randPos.x, 1f, randPos.y);
		GameObject newCar = PhotonNetwork.Instantiate ("SCCar" + car.model, spawnPos, Quaternion.identity, 0);

		PhotonNetwork.RaiseEvent (0, null, true, null);

	}

	public override void OnLeftRoom() {
		Debug.Log ("left room");
		PhotonNetwork.LeaveLobby ();
	}

	public override void OnLeftLobby() {
		Debug.Log ("left lobby");
	}
}
