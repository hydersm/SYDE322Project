using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SCGarageUIController : MonoBehaviour {

	public Material[] carBodyMaterials;
	public Material[] excludeMaterials;
	public GameObject car;
	public Button saveButton;
	public Dropdown colorDropdown;
	public Dropdown modelDropdown;
	int activeColor;
	int activeModel;

	void Start() {
		modelDropdown.value = SCNetworkManager.instance.car.model;
		if (car == null) {
			UpdateModel ();
		}
		colorDropdown.value = SCNetworkManager.instance.car.color;
	}

	public void ColorDropDownPressed(Dropdown dropdown) {
		activeColor = dropdown.value;
		UpdateColor ();
	}

	public void ModelDropDownPressed(Dropdown dropdown) {
		activeModel = dropdown.value;
		UpdateModel ();
		UpdateColor ();
	}

	private void UpdateModel() {
		if (car != null) {
			Destroy (car);
		}
		car = (GameObject) Instantiate (Resources.Load("SCCar" + activeModel + "Preview"));
		Debug.Log (car);
	}

	private void UpdateColor() {
		if (activeModel == 0) {
			Material[] temp = { carBodyMaterials [activeColor], carBodyMaterials [activeColor] };
			car.GetComponent<SCCarPreviewData> ().carBodyParts [0].GetComponent<Renderer> ().materials = temp;
		} else if (activeModel == 1) {
			Material color = carBodyMaterials [activeColor];
			foreach (GameObject carPart in car.GetComponent<SCCarPreviewData>().carBodyParts) {
				Material[] temp = carPart.GetComponent<Renderer> ().materials;
				for (int i = 0; i < temp.Length; i++) {
					bool add = true;

					for (int j = 0; j < excludeMaterials.Length; j++) {
						if (temp [i].ToString ().Substring(0, 8) == excludeMaterials [j].ToString ().Substring(0,8)) {
							add = false;
						}
					}

					if (add) {
						temp [i] = color;
					}
				}

				carPart.GetComponent<Renderer> ().materials = temp;
			}
		}
	}

	public void SaveButtonPressed(){
		SCNetworkManager.instance.car.color = activeColor;
		SCNetworkManager.instance.car.model = activeModel;
		saveButton.interactable = false;
		StartCoroutine (SaveButtonCoroutine());
	}

	IEnumerator SaveButtonCoroutine() {
		string url = "https://guarded-thicket-12107.herokuapp.com/car/" + SCNetworkManager.instance.car.id + "/";

		WWWForm form = new WWWForm ();
		form.AddField ("player", SCNetworkManager.instance.account.id);
		form.AddField ("color", SCNetworkManager.instance.car.color);
		form.AddField ("model", SCNetworkManager.instance.car.model);

		WWW www = new WWW (url, form);
		yield return www;

		if (!string.IsNullOrEmpty (www.error)) {
			Debug.Log ("Car save Failed!");
			Debug.Log ("car post response: " + www.text);
			saveButton.interactable = true;
		} else {
			Debug.Log ("Car saved");
			Debug.Log ("car post response: " + www.text);
			SCSceneController.instance.LoadLevel ("MainMenuScene");
		}
	}

}
