using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;

public class SCCarInit : Photon.PunBehaviour {

	public Material[] carBodyMaterials;
	public Material[] excludeMaterials;
	public GameObject[] carBodyParts;
	public int model;

	// Use this for initialization
	void Start () {
		if (photonView.isMine) {
			SCCameraRootController.firstInstance.target = transform.Find ("CameraRootTarget");
		}

		if (model == 0) {
			Material color = carBodyMaterials [(int)photonView.owner.customProperties ["color"]];
			Material[] temp = { color, color };
			foreach (GameObject carPart in carBodyParts) {
				carPart.GetComponent<Renderer> ().materials = temp;
			}
		} else if (model == 1) {
			Material color = carBodyMaterials [(int)photonView.owner.customProperties ["color"]];
			foreach (GameObject carPart in carBodyParts) {
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
	
	// Update is called once per frame
	void Update () {
	
	}
}
