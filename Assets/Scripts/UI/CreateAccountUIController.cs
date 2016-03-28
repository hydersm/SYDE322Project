using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateAccountUIController : MonoBehaviour {

	public string username;
	public string password;
	public string confirmPassword;
	public Button createButton;
	public GameObject createFailMessage;

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
//		SCSceneController.instance.username = this.username;
//		SCSceneController.instance.LoadLevel ("MainMenuScene");
		createButton.interactable = false;
		createFailMessage.SetActive (false);
		StartCoroutine(CreateAccountButtonPressedCoroutine());

	}

	IEnumerator CreateAccountButtonPressedCoroutine() {
		if (password == confirmPassword) {
			string url = "https://guarded-thicket-12107.herokuapp.com/account/";

			WWWForm form = new WWWForm ();
			form.AddField ("name", username);
			form.AddField ("password", password);

			WWW www = new WWW (url, form);
			yield return www;

			if (!string.IsNullOrEmpty (www.error)) {
				Debug.Log ("Acount Creation Failed!");
				createButton.interactable = true;
				createFailMessage.SetActive (true);
			} else {
				Debug.Log ("Account Created");
				Debug.Log ("creation post response: " + www.text);
				SCNetworkManager.instance.account = JsonUtility.FromJson<SCAccount> (www.text);
				SCSceneController.instance.LoadLevel ("MainMenuScene");
			}
		}
	}
}
