using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public enum avatarDisplayState {hide, show, fade}

[System.Serializable]
public struct Dialogue {
	public Sprite[] avatars;
	public avatarDisplayState[] avatarStates;
	public Sprite background;
	public string text;
}


public class DialogueManager : MonoBehaviour {
	public Dialogue[] dialogues;

	public GameObject[] avatars;
	public GameObject background;
	public GameObject textPanel;
	private int currentDialogueInd = -1;

	public void Skip() {
		LevelManager.S.GoToNextLevel ();
	}
	private void nextDialogue () {
		currentDialogueInd++;
		GetComponent<AudioSource> ().Play();
		if (currentDialogueInd >= dialogues.Length) {
			LevelManager.S.GoToNextLevel ();
			return;
		}
		for (int i=0; i < avatars.Length; i++) {
			setAvatarSprite (dialogues [currentDialogueInd].avatars [i], i);
			setAvatarDisplay (dialogues [currentDialogueInd].avatarStates [i], i);
			setBackground (dialogues [currentDialogueInd].background);
		}
		textPanel.GetComponent<Text> ().text = dialogues [currentDialogueInd].text;
	}

	private void setAvatarSprite (Sprite img, int avatarInd) {
		avatars[avatarInd].GetComponent<Image> ().sprite = img;
	}

	private void setBackground(Sprite img) {
		background.GetComponent<Image> ().sprite = img;
	}

	private void setAvatarDisplay(avatarDisplayState opt, int avatarInd) {
		switch (opt) {
		case avatarDisplayState.hide:
			avatars[avatarInd].GetComponent<Image> ().enabled = false;
			break;
		case avatarDisplayState.show:
			avatars[avatarInd].GetComponent<Image> ().enabled = true;
			avatars[avatarInd].GetComponent<Image> ().color = Color.white;
			break;
		case avatarDisplayState.fade:
			avatars[avatarInd].GetComponent<Image> ().enabled = true;
			avatars[avatarInd].GetComponent<Image> ().color = Color.gray;
			break;
		}
	}


	// Use this for initialization
	void Start () {
		nextDialogue ();
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space)) {
			nextDialogue ();
		}
	}

}
