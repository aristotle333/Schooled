using UnityEngine;
using System.Collections;

public class LevelSelectButton : MonoBehaviour {
	public int level;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GoToLevel() {
		LevelManager.S.JumpToLevel (level);
	}

	public void GoBack() {
		LevelManager.S.QuitToMainMenu ();
	}
}
