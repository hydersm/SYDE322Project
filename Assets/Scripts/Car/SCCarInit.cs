using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;

public class SCCarInit : Photon.PunBehaviour {

	public Material[] carBodyMaterials;
	public GameObject carBody;

	// Use this for initialization
	void Start () {
		if (photonView.isMine) {
			SCCameraRootController.firstInstance.target = transform.Find ("CameraRootTarget");
		}
		Material color = carBodyMaterials [(int)photonView.owner.customProperties ["color"]];
		Material[] temp = { color, color };
		carBody.GetComponent<Renderer> ().materials = temp;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
