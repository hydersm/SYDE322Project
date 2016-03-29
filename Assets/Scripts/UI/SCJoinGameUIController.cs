using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SCJoinGameUIController : MonoBehaviour {

	public Transform content;
	public Color activeButtonColor;
	public Color notActiveButtonColor;

	private int size = 8;
	private RectTransform contentRect;
	private GameObject[] buttons;
	private int activeButton = -1;

	void Start() {
		StartCoroutine (RoomListCoroutine ());
	}

	IEnumerator RoomListCoroutine(){
		while (true) {
			if (SCNetworkManager.instance.isConnectedToMaster) {
				SCNetworkManager.instance.JoinDefaultLobbyAndGetRoomList (roomListCallback);
				yield break;
			}
			yield return null;
		}
	}

	public void roomListCallback(RoomInfo[] roomInfos) {
		if (buttons != null) {
			for (int i = 0; i < buttons.Length; i++) {
				Destroy (buttons [i]);
			}
		}
			
		buttons = new GameObject[roomInfos.Length];

		for (int i = 0; i < roomInfos.Length; i++) {
			if (roomInfos [i].playerCount < roomInfos[i].maxPlayers && roomInfos[i].open) {
				buttons [i] = (GameObject)Instantiate (Resources.Load ("GameListItem"));
				buttons [i].transform.Find ("Text").gameObject.GetComponent<Text> ().text = roomInfos [i].name + "(" + roomInfos [i].playerCount + "/" + roomInfos [i].maxPlayers + ")";
				buttons [i].transform.SetParent (content);
				int itemp = i;
				buttons [i].GetComponent<Button> ().onClick.AddListener (delegate {
					buttonClicked (itemp);
				});
			}
		}

		contentRect = content.GetComponent<RectTransform> ();
	}

	public void buttonClicked(int idx) {
		Debug.Log ("buttons length: " + buttons.Length);
		Debug.Log ("idx: " + idx);
		for (int i = 0; i < buttons.Length; i++) {
			ColorBlock colors = buttons [i].GetComponent<Button> ().colors;
			colors.normalColor = notActiveButtonColor;
			colors.highlightedColor = notActiveButtonColor;
			colors.pressedColor = notActiveButtonColor;
			colors.disabledColor = notActiveButtonColor;
			buttons [i].GetComponent<Button> ().colors = colors;
			buttons [i].transform.Find ("Text").gameObject.gameObject.GetComponent<Text> ().color = activeButtonColor;

		}

		activeButton = idx;
		ColorBlock colors2 = buttons [idx].GetComponent<Button> ().colors;
		colors2.normalColor = activeButtonColor;
		colors2.highlightedColor = activeButtonColor;
		colors2.pressedColor = activeButtonColor;
		colors2.disabledColor = activeButtonColor;
		buttons [idx].GetComponent<Button> ().colors = colors2;
		buttons [idx].transform.Find ("Text").gameObject.gameObject.GetComponent<Text> ().color = notActiveButtonColor;

	}

	public void JoinButtonPressed() {
		if (activeButton >= 0) {
			SCNetworkManager.instance.JoinRoom(buttons [activeButton].transform.Find ("Text").gameObject.GetComponent<Text> ().text.Split('(')[0]);
		}
	}

	void Update () {
		if (buttons != null) {
			for (int i = 0; i < buttons.Length; i++) {
				if (buttons [i] != null) {
					RectTransform buttonRect = buttons [i].transform.GetComponent<RectTransform> ();
					buttonRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Top, i * 30f, 30f);
					buttonRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Left, 0, contentRect.rect.width);
				}
			}
		}
	}
}
