using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager
{
    private const string MAIN_MENU_SCENE = "_Scene_MainMenu";
	private const string LEVEL_MENU_SCENE = "_Scene_LevelSelect";

    public static LevelManager S = new LevelManager();

    private List<string> scenes = new List<string>
    {
		"_Scene_Dialogue_Tutorial_0", "_Scene_Tutorial0",
		"_Scene_Dialogue_Tutorial_1", "_Scene_Tutorial1",
		"_Scene_Dialogue_Tutorial_2", "_Scene_Tutorial2",
		"_Scene_Dialogue_Tutorial_3", "_Scene_Tutorial3",
		"_Scene_Dialogue_Tutorial_Big", "_Scene_Tutorial_BigNode",
		"_Scene_Dialogue0", "_Scene_Level0",
		"_Scene_Dialogue1", "_Scene_Level1",
		"_Scene_Dialogue2", "_Scene_Level2",
		"_Scene_Dialogue3", "_Scene_Level3",
		"_Scene_Dialogue4", "_Scene_Level4",
		"_Scene_Dialogue5", "_Scene_Level5",
		"_Scene_Dialogue6", "_Scene_Level6"
    };

    // The level that each unit type is unlocked at
    private Dictionary<UnitType, int> unitsIntroducedAt = new Dictionary<UnitType, int>
    {
        { UnitType.UnitBasic,       0 },
        { UnitType.Archer,          1 },
        { UnitType.Engineer,        2 },
        { UnitType.Balloon,         3 },
    };
    // TODO: do something similar for unlocking overseer powers

    private HashSet<UnitType> unitsUnlocked;
    public HashSet<UnitType> UnitsUnlocked { get { return unitsUnlocked; } }

    public int CurrentLevel { get; private set; }

    private LevelManager()
    {
        CurrentLevel = 0;
    }

    public void GoToNextLevel()
    {
        JumpToLevel(CurrentLevel + 1);
    }

    public void JumpToLevel(int level)
    {
        CurrentLevel = level;
		Node.interactible = true;
		CandyFactory.numOfCandyFactory = 0;
        var unlocked = unitsIntroducedAt.Where(kv => kv.Value <= level).Select(kv => kv.Key);
        unitsUnlocked = new HashSet<UnitType>(unlocked);

        SceneManager.LoadScene(scenes[CurrentLevel]);
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }

	public void GoToLevelSelectMenu(){
		SceneManager.LoadScene(LEVEL_MENU_SCENE);
	}

    public void RestartLevel()
    {
		CandyFactory.numOfCandyFactory = 0;
        SceneManager.LoadScene(this.scenes[CurrentLevel]);
    }
}
