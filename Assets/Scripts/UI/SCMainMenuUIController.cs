using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SCMainMenuUIController : MonoBehaviour {

	public UnityEngine.UI.Text userNameTextBox;
	public Button createGameButton;
	public Button joinGameButton;
	public Button garageButton;

	// Use this for initialization
	void Start () {
		userNameTextBox.text = "User: " + SCNetworkManager.instance.account.name + "   ";
		if (SCNetworkManager.instance.car.id == 0) {
			createGameButton.interactable = false;
			joinGameButton.interactable = false;
			garageButton.interactable = false;
			StartCoroutine (GetCarInfoCoroutine ());
		}
	}
	
	public void CreateGameButtonPressed() {
		SCSceneController.instance.LoadLevel ("CreateGameScene");
	}

	public void JoinGameButtonPressed() {
		SCSceneController.instance.LoadLevel ("JoinGameScene");
	}

	public void GarageButtonPressed() {
		SCSceneController.instance.LoadLevel ("GarageScene");
	}

	IEnumerator GetCarInfoCoroutine() {
		string url = "https://guarded-thicket-12107.herokuapp.com/active_car/" + SCNetworkManager.instance.account.id;

		WWW www = new WWW (url);
		yield return www;

		Debug.Log ("carinfo get response: " + www.text);
		SCNetworkManager.instance.car = JsonUtility.FromJson<SCCar> (www.text);
		createGameButton.interactable = true;
		joinGameButton.interactable = true;
		garageButton.interactable = true;
	}
}
