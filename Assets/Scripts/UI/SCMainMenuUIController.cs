using UnityEngine;
using System.Collections;

public class SCMainMenuUIController : MonoBehaviour {

	public UnityEngine.UI.Text userNameTextBox;

	// Use this for initialization
	void Start () {
		userNameTextBox.text = "User: " + SCNetworkManager.instance.account.name + "   ";
	}
	
	public void CreateGameButtonPressed() {
		SCSceneController.instance.LoadLevel ("CreateGameScene");
	}

	public void JoinGameButtonPressed() {
		SCSceneController.instance.LoadLevel ("JoinGameScene");
	}
}
