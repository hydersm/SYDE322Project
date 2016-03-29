using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon;

public class SCMainSceneUi : Photon.PunBehaviour {

	public GameObject escapeMenu;
	public GameObject loseScreen;
	public GameObject winScreen;
	public Text playerListText;
	public Text infoText;
	public int activePlayers;

	private float lastDotTime;
	private int dotCount = 0;

	private float startTime = -1f;

	// Use this for initialization
	void Start () {
		PhotonNetwork.OnEventCall += this.PlayerJoinedEvent;
		PhotonNetwork.OnEventCall += this.PlayerLost;
		PhotonNetwork.OnEventCall += this.RestartGame;
	}

	public override void OnJoinedRoom() {
		loseScreen.SetActive (false);
		winScreen.SetActive (false);
		UpdatePlayerList ();
		if (PhotonNetwork.playerList.Length == PhotonNetwork.room.maxPlayers) {
			SCNetworkManager.instance.canControlCar = true;
			infoText.text = "Start!";
			startTime = Time.time;
			activePlayers = PhotonNetwork.room.maxPlayers;
		} else {
			infoText.text = "Waiting for players to join";
			lastDotTime = Time.time;
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			escapeMenu.SetActive (!escapeMenu.activeSelf);
		}

		UpdateInfo ();
	}

	public void ExitButtonPressed(){
		PhotonNetwork.RaiseEvent (3, null, true, null);
		PhotonNetwork.LeaveRoom ();
		SCSceneController.instance.LoadLevel ("MainMenuScene");
	}

	public void PlayerJoinedEvent(byte eventCode, object content, int senderid) {
		if (eventCode == 0) {
			if (PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers) {
				PhotonNetwork.room.open = false;
			}
			UpdatePlayerList ();

			if (PhotonNetwork.playerList.Length == PhotonNetwork.room.maxPlayers) {
				SCNetworkManager.instance.canControlCar = true;
				infoText.text = "Start!";
				startTime = Time.time;
				activePlayers = PhotonNetwork.room.maxPlayers;
			} else {
				if (infoText.text.Length < 27 || infoText.text.Substring (0, 27) != "Waiting for players to join") {
					infoText.text = "Waiting for players to join";
				}
				lastDotTime = Time.time;
			}

		}
	}

	public void PlayerLost(byte eventCode, object content, int senderid) {
		if (eventCode == 1 || eventCode == 3) {
			activePlayers--;
			if (activePlayers == 1 && !loseScreen.activeSelf) {
				StartCoroutine (ShowWinScreen ());
			}
		}
	}

	public void RestartGame(byte eventCode, object content, int senderid) {
		if (eventCode == 2) {
			SCNetworkManager.instance.OnJoinedRoom ();
			this.OnJoinedRoom ();
		}
	}

	private void UpdatePlayerList(){
		PhotonPlayer[] players = PhotonNetwork.playerList;
		string playerListString = "";

		foreach (PhotonPlayer player in players) {
			playerListString += player.customProperties ["name"] + "\n";
		}

		playerListText.text = playerListString;
	}

	private void UpdateInfo() {
		if (infoText.text.Length >= 27 && infoText.text.Substring(0, 27) ==  "Waiting for players to join") {
			string info = "Waiting for players to join";
			for (int i = 0; i < dotCount; i++) {
				info += ".";
			}

			if ((Time.time - lastDotTime) > 1f) {
				dotCount++;
				if (dotCount > 3) {
					dotCount = 0;
				}
				lastDotTime = Time.time;
			}
			infoText.text = info;

		} else if (infoText.text == "Start!") {
			if ((Time.time - startTime) > 3f) {
				infoText.text = "";
			}
		}
	}

	public void ShowLoseScreen() {
		activePlayers--;
		winScreen.SetActive (false);
		loseScreen.SetActive (true);
		PhotonNetwork.RaiseEvent (1, null, true, null);
	}

	IEnumerator ShowWinScreen() {
		loseScreen.SetActive (false);
		winScreen.SetActive (true);
		yield return new WaitForSeconds(3f);
		PhotonNetwork.RaiseEvent (2, null, true, null);
		PhotonNetwork.DestroyPlayerObjects (PhotonNetwork.player.ID);
		SCNetworkManager.instance.OnJoinedRoom ();
		this.OnJoinedRoom ();
	}
}
