using UnityEngine;
using System.Collections;
using System;

public class SCCreateGameUIController :  MonoBehaviour{

	public string gameName;
	public int numberOfPlayer;

	public void SetGameName(UnityEngine.UI.Text gameNameTextBox) {
		this.gameName = gameNameTextBox.text;
	}

	public void SetNumberOfPlayers(UnityEngine.UI.Text numBox) {
		this.numberOfPlayer = Int32.Parse (numBox.text);
	}

	public void CreateGameButtonPressed() {
		StartCoroutine (CreateGameCoroutine ());
	}

	IEnumerator CreateGameCoroutine(){
		while (true) {
			if (SCNetworkManager.instance.isConnectedToMaster) {
				SCNetworkManager.instance.JoinDefaultLobbyAndCreateGame (gameName, numberOfPlayer);
				yield break;
			}
			yield return null;
		}
	}
}
