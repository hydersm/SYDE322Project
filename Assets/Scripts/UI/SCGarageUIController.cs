using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SCGarageUIController : MonoBehaviour {

	public Material[] carBodyMaterials;
	public GameObject carbody;
	int activeColor;

	void Start() {
		Material[] temp = { carBodyMaterials [0], carBodyMaterials [0] };
		carbody.GetComponent<Renderer> ().materials = temp;
	}

	public void DropDownPressed(Dropdown dropdown) {
		Material[] temp = { carBodyMaterials [dropdown.value], carBodyMaterials [dropdown.value] };
		carbody.GetComponent<Renderer> ().materials = temp;
		activeColor = dropdown.value;
		Debug.Log (activeColor);
	}

	public void SaveButtonPressed(){
		SCCar newCar = new SCCar ();
		newCar.color = activeColor;
		SCNetworkManager.instance.car = newCar;
		SCSceneController.instance.LoadLevel ("MainMenuScene");
	}

}
