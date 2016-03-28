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

	private bool createGame;
	private string gameName;
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
		Debug.Log ("Connected to Master.\n Joining Default Lobby.");
		base.OnConnectedToMaster ();
		isConnectedToMaster = true;
	}

	public void JoinDefaultLobbyAndCreateGame(string name) {
		createGame = true;
		gameName = name;
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
//			RoomOptions roomOptions = new RoomOptions ();
//			roomOptions.isVisible = true;
//			roomOptions.isOpen = true;
//			roomOptions.cleanupCacheOnLeave = true;
//			roomOptions.maxPlayers = 6;
//			roomOptions.

			SCSceneController.instance.LoadLevel ("MainScene");
//			PhotonNetwork.CreateRoom (gameName, roomOptions, TypedLobby.Default);
			Debug.Log("gamename: " + gameName);
			PhotonNetwork.CreateRoom(gameName);
		}
	}

	public override void OnReceivedRoomListUpdate() {
		Debug.Log ("Photon Room Update Called!");
		RoomInfo[] roomInfos = PhotonNetwork.GetRoomList ();
		roomListCallback (roomInfos);
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

		Vector2 randPos = UnityEngine.Random.insideUnitCircle * 50;
		Vector3 spawnPos = new Vector3 (randPos.x, 1f, randPos.y);
		GameObject newCar = PhotonNetwork.Instantiate ("SCCar3", spawnPos, Quaternion.identity, 0);

	}
}
