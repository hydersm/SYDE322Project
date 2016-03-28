using UnityEngine;
using System.Collections;

public class SCLoginUIController : MonoBehaviour {

	public string username;
	public string password;

	public void SetUserName(UnityEngine.UI.Text uiText) {
		this.username = uiText.text;
	}

	public void SetPassword(UnityEngine.UI.Text uiText) {
		this.password = uiText.text;
	}

	public void LoginButtonPressed() {
		SCSceneController.instance.username = this.username;
		SCSceneController.instance.LoadLevel ("MainMenuScene");
	}

	public void CreateAccountButtonPressed() {
		SCSceneController.instance.LoadLevel ("CreateAccountScene");
	}
}
