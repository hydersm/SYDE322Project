using UnityEngine;
using System.Collections;

public class SCCarRespawn : MonoBehaviour {

	public SCMainSceneUi uicontroller;

    void OnTriggerExit(Collider car)
    {
		if (car.transform.root.gameObject.GetComponent<PhotonView> ().isMine) {
			PhotonNetwork.Destroy (car.transform.root.gameObject);
			uicontroller.ShowLoseScreen ();
		}

    }
}
