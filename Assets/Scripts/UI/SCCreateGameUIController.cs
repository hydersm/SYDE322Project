using UnityEngine;
using System.Collections;

public class SCCreateGameUIController :  MonoBehaviour{

	public string gameName;

	public void SetGameName(UnityEngine.UI.Text gameNameTextBox) {
		this.gameName = gameNameTextBox.text;
	}

	public void CreateGameButtonPressed() {
		StartCoroutine (CreateGameCoroutine ());
	}

	IEnumerator CreateGameCoroutine(){
		while (true) {
			if (SCNetworkManager.instance.isConnectedToMaster) {
				SCNetworkManager.instance.JoinDefaultLobbyAndCreateGame (gameName);
				yield break;
			}
			yield return null;
		}
	}
}
