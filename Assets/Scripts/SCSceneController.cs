using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SCSceneController : MonoBehaviour {

	public static SCSceneController instance;

	public string username;

	public void Awake() {
		if (instance == null) {
			DontDestroyOnLoad (gameObject);
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	public void LoadLevel(string name) {
		SceneManager.LoadScene (name);
	}
}
