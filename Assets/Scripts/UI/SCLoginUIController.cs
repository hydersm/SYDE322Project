using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.UI;


public class SCLoginUIController : MonoBehaviour {

	public string username;
	public string password;
	public GameObject failTextBox;
	public GameObject loginButton;
	public GameObject createButton;

	public void SetUserName(UnityEngine.UI.Text uiText) {
		this.username = uiText.text;
	}

	public void SetPassword(UnityEngine.UI.Text uiText) {
		this.password = uiText.text;
	}

	public void LoginButtonPressed() {

		failTextBox.SetActive (false);
		loginButton.GetComponent<Button> ().interactable = false;
		createButton.GetComponent<Button> ().interactable = false;
		StartCoroutine (LoginButtonPressedCoroutine ());

	}

	IEnumerator LoginButtonPressedCoroutine() {
		string url = "https://guarded-thicket-12107.herokuapp.com/account/?name=" + this.username + "&password=" + this.password;

		WWW www = new WWW (url);
		yield return www;

		Debug.Log ("login get response: " + www.text);
		SCNetworkManager.instance.account = JsonUtility.FromJson<SCAccount> (www.text);
		if (SCNetworkManager.instance.account.name == null) {
			failTextBox.SetActive (true);
			loginButton.GetComponent<Button> ().interactable = true;
			createButton.GetComponent<Button> ().interactable = true;
		} else {
			SCSceneController.instance.LoadLevel ("MainMenuScene");
		}

	}

	public void CreateAccountButtonPressed() {
		SCSceneController.instance.LoadLevel ("CreateAccountScene");
	}
}
