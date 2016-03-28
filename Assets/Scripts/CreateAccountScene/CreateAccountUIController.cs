using UnityEngine;
using System.Collections;

public class CreateAccountUIController : MonoBehaviour {

	public string username;
	public string password;
	public string confirmPassword;

	public void SetUserName(UnityEngine.UI.Text uiText) {
		this.username = uiText.text;
	}

	public void SetPassword(UnityEngine.UI.Text uiText) {
		this.password = uiText.text;
	}

	public void SetConfirmPassword(UnityEngine.UI.Text uiText) {
		this.confirmPassword = uiText.text;
	}

	public void CreateAccountButtonPressed() {
		SCSceneController.instance.username = this.username;
		SCSceneController.instance.LoadLevel ("MainMenuScene");
	}
}
