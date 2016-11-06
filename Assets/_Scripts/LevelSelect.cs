using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelSelect : MonoBehaviour
{
    private bool flag = false;
	void Update()
    {
        if (flag) // So we don't double load the scene? Unsure if this is possible but it sounds bad
            return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
			LevelManager.S.GoToLevelSelectMenu ();
            flag = true;
        }
	}

	public void StartFromBeginning () {
		LevelManager.S.JumpToLevel (0);
	}

	public void SelectLevel () {
		LevelManager.S.GoToLevelSelectMenu ();
	}
}
