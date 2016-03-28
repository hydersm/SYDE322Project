using UnityEngine;
using System.Collections;

public class SCMainMenuUIController : MonoBehaviour {

	public UnityEngine.UI.Text userNameTextBox;

	// Use this for initialization
	void Start () {
		userNameTextBox.text = "User: " + SCSceneController.instance.username + "   ";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
