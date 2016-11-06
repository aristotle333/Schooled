using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System;

public class UI : MonoBehaviour
{
    public static UI S;
    public int startingRes = 200;
    Text resourceNum;
    public AudioSource sound;

    public GameObject BarReference;
    public GameObject lollipopReference;
    public GameObject CreateUnitBarRef;
    public GameObject PromptReference;
	public GameObject PauseMenuReference;
    public GameObject WaveButtonReference;
    public GameObject OverseerMenu;
    public GameObject   UpgradeMenu;
    private bool        UpgradeMenuState = false;       // UpgradeMenuState is false when it is inactive, otherwise it is set to true

    public Text WaveTextReference; 
    public Text HousesVisRef;
    public Text UnitsPassedRef;
    public Text UpgradeButtonText;

    public Text waveButtonText;
    public Button waveButton;

    public GameObject WaveTextDisplay;
    private float waitingTimeWaveText = 0.03f;

    [Header("Hover Text references")]
    public GameObject KnightHover;
    public GameObject ArcherHover;
    public GameObject FlyingHover;
    public GameObject EngineerHover;
    public GameObject UndoHover;

    void Awake()
    {
        S = this;
        sound = GetComponent<AudioSource>();
        PromptReference.SetActive(false);
        Time.timeScale = 1f;

        resourceNum = transform.FindChild("Bar").FindChild("Resource Counter").GetComponent<Text>();
        waveButton = WaveButtonReference.GetComponent<Button>();
        waveButtonText = WaveButtonReference.transform.FindChild("Text").GetComponent<Text>();
        resourceNum.text = startingRes.ToString();
        HousesVisRef.GetComponent<Text>();
        UnitsPassedRef.GetComponent<Text>();
    }
    void Start ()
    {
        sound.Play();
        updateWaveNumber(1);
        HousesVisRef.text = "Houses Visited 0/" + TownGates.S.numOfHousesToVisit;
        UnitsPassedRef.text = "Units Passed 0/" + TownGates.S.unitsRequiredToPass;
    }

    public void changeResources(int add)
    {
		ResourceManager.S.AddResources (add);
    }

    public void attemptPurchase(string clicked)
    {
        UnitType unit = (UnitType)Enum.Parse(typeof(UnitType), clicked);
        if (ResourceManager.S.TryDoPurchase(unit))
        {
            PlaySound("Chaching");
            Spawn.S.SpawnUnit(unit);
            return;
        }

        PlaySound("Reject");
    }
    public void startWave()
    {
        StartCoroutine(startWaveMusic());
        CreateUnitBarRef.SetActive(false);
        OverseerMenu.SetActive(true);
        UpgradeMenu.SetActive(false);
        WaveManager.S.StartWave();
    }
    IEnumerator startWaveMusic()
    {
        sound.Stop();
        sound.clip = Resources.Load("Sounds/Music") as AudioClip;
        PlaySound("WaveStart");
        yield return new WaitForSeconds(2.6f);
        sound.Play();
    }
    public void updateResourceValue(int value)
    {
        resourceNum.text = value.ToString();
    }

    public void updateWaveNumber(int wave) {
        WaveTextReference.text = "Wave " + wave + "/" + WaveManager.S.numberOfWaves;
    }

    // Deactivates all elements of the UI and activates the Prompt element. If win is true display winning prompt, otherwise display game over prompt
    public void DisplayPrompt(bool win) {
        BarReference.SetActive(false);
        lollipopReference.SetActive(false);
        CreateUnitBarRef.SetActive(false);
        OverseerMenu.SetActive(false);
        PromptReference.SetActive(true);
        Time.timeScale = 0f;

        if (win) {
            DisplayWinningCondition();
        }
        else {
            DisplayLosingCondition();
        }
    }

    public void UpdateUIHouses(int housesVisited) {
        HousesVisRef.text = "Houses Visited " + housesVisited + "/" + TownGates.S.numOfHousesToVisit;
    }

    public void UpdateUnitsPassed(int unitsPassed) {
        UnitsPassedRef.text = "Units Passed " + unitsPassed + "/" + TownGates.S.unitsRequiredToPass;
        if (TownGates.S.unitsPassed >= TownGates.S.unitsRequiredToPass) {
            DisplayPrompt(true);
        }
    }



    void DisplayWinningCondition() {
        string textToDisplay = "You Win!";
        //float timeRemainingNow = Timer.S.timeRemaining; // Commenting out because Timer won't exist soon
        //string timeRemaining = timeRemainingNow.ToString("F2");
        PromptReference.transform.FindChild("PromptText").gameObject.GetComponent<Text>().text = textToDisplay;
		PromptReference.transform.FindChild ("Continue").gameObject.SetActive (true);
        //PromptReference.transform.FindChild("Stats").gameObject.GetComponent<Text>().text = "Time Remaining: " + timeRemaining;
    }

    void DisplayLosingCondition() {
        string textToDisplay = "Game Over!";
        PromptReference.transform.FindChild("PromptText").GetComponent<Text>().text = textToDisplay;
        PromptReference.transform.FindChild("Stats").gameObject.SetActive(false);
		PromptReference.transform.FindChild ("Continue").gameObject.SetActive (false);
    }

    public void PlayAgain() {
        LevelManager.S.RestartLevel();
    }

    public void QuitMainMenu() {
        // TODO, Change this to the correct scene once we have a mainMenu
        LevelManager.S.QuitToMainMenu();
    }

    public void Continue() {
        // TODO, Change this to the name of the next scene
        LevelManager.S.GoToNextLevel();
    }

    public void PlaySound(string soundName)
    {
        sound.PlayOneShot(Resources.Load("Sounds/" + soundName) as AudioClip);
    }

    public void OpenUpgradeMenu() {
        PlaySound("MouseClick");
        if (!UpgradeMenuState) {
            UpgradeMenu.SetActive(true);
            UpgradeMenuState = true;
            UpgradeButtonText.text = "Close Upgrade Menu";
        }
        else {
            UpgradeMenu.SetActive(false);
            UpgradeMenuState = false;
            UpgradeButtonText.text = "Upgrade";
        }
    }

    public void WaveTextDisplayFunction(bool increaseText) {
        Text txt = getTextChild(WaveTextDisplay);
        if (increaseText) {
            txt.text = "Wave " + WaveManager.S.WaveNumber + " Terminated!";
            WaveTextDisplay.SetActive(true);
            WaveTextDisplay.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            StartCoroutine(WaveTextCoroutine(WaveTextDisplay, true));
        }
        else {
            txt.text = "Starting Wave " + WaveManager.S.WaveNumber;
            WaveTextDisplay.SetActive(true);
            WaveTextDisplay.transform.localScale = new Vector3(1, 1, 1);
            StartCoroutine(WaveTextCoroutine(WaveTextDisplay, false));
        }
    }

    private IEnumerator WaveTextCoroutine(GameObject GO, bool increaseSize) {
        if (increaseSize) {
            while (GO.transform.localScale.x <= 1f) {
                GO.transform.localScale += new Vector3(0.04f, 0.04f, 0.04f);
                yield return new WaitForSeconds(waitingTimeWaveText);
            }
            if (GO.transform.localScale.x >= 1f) {
                yield return new WaitForSeconds(40 * waitingTimeWaveText);
                UI.S.sound.Play();
                GO.SetActive(false);
                StopCoroutine(WaveTextCoroutine(GO, true));
            }
        }
        else {
            yield return new WaitForSeconds(25 * waitingTimeWaveText);
            while (GO.transform.localScale.x >= 0.1f) {
                GO.transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
                yield return new WaitForSeconds(waitingTimeWaveText);
            }
            if (GO.transform.localScale.x <= 0.1f) {
                GO.SetActive(false);
                StopCoroutine(WaveTextCoroutine(GO, false));
            }
        }
    }

    public void GatesTextDisplayFunction() {
        Text txt = getTextChild(WaveTextDisplay);
        txt.text = "The Gates Are Open!";
        WaveTextDisplay.SetActive(true);
        WaveTextDisplay.transform.localScale = new Vector3(1, 1, 1);
        StartCoroutine(WaveTextCoroutine(WaveTextDisplay, false));
    }

    public IEnumerator GatesAreOpenRoutine(GameObject GO) {
        yield return new WaitForSeconds(40 * waitingTimeWaveText);
        while (GO.transform.localScale.x >= 0.1f) {
            GO.transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
            yield return new WaitForSeconds(waitingTimeWaveText);
        }
        if (GO.transform.localScale.x <= 0.1f) {
            GO.SetActive(false);
            StopCoroutine(GatesAreOpenRoutine(GO));
        }        
    }


    #region Hover Button Prompts

    public void UndoUnitSpawn()
    {
        PlaySound("MouseClick");
        Spawn.S.UndoSpawn();
    }

    public void KnightHoverFunction(bool active) {
        if (active) {
            KnightHover.SetActive(true);
            Text txt = getTextChild(KnightHover);
            txt.text = "<size=27>That Guy</size>\n" +
                       "<color=red>Health: " + Upgrade.S.getUpgradedHealth(UnitType.UnitBasic) + "</color>\n" +
                       "<color=green>Damage: " + Upgrade.S.getUpgradedDamage(UnitType.UnitBasic) + "</color>\n\n" +
                       "<size=20>You know...he's that guy.</size>";
        }
        else {
            KnightHover.SetActive(false);
        }
    }

    public void ArcherHoverFunction(bool active) {
        if (active) {
            ArcherHover.SetActive(true);
            Text txt = getTextChild(ArcherHover);
            txt.text = "<size=27>Spitballer</size>\n" +
                       "<color=red>Health: " + Upgrade.S.getUpgradedHealth(UnitType.Archer) + "</color>\n" +
                       "<color=green>Damage: " + Upgrade.S.getUpgradedDamage(UnitType.Archer) + "</color>\n\n" +
                       "<size=20>Her spits will miss no teacher.</size>";
        }
        else {
            ArcherHover.SetActive(false);
        }
    }

    public void FlyingHoverFunction(bool active) {
        if (active) {
            FlyingHover.SetActive(true);
            Text txt = getTextChild(FlyingHover);
            txt.text = "<size=27>Class Clown</size>\n" +
                       "<color=red>Health: " + Upgrade.S.getUpgradedHealth(UnitType.Balloon) + "</color>\n" +
                       "<color=blue>Speed: " + Upgrade.S.getUpgradedSpeed(UnitType.Balloon) + "</color>\n\n" +
                       "<size=20>He can pass through everything very fast...except his exams.</size>";
        }
        else {
            FlyingHover.SetActive(false);
        }
    }

    public void EngineerHoverFunction(bool active) {
        if (active) {
            EngineerHover.SetActive(true);
            Text txt = getTextChild(EngineerHover);
            txt.text = "<size=27>Nerd</size>\n" +
                       "<color=red>Health: " + Upgrade.S.getUpgradedHealth(UnitType.Engineer) + "</color>\n\n" +
                       //"<color=blue>Speed: " + Upgrade.S.getUpgradedSpeed(UnitType.Balloon) + "</color>\n\n" +
                       "<size=20>That guy with the glasses.\n P.S. He loves vegetables.</size>";
        }
        else {
            EngineerHover.SetActive(false);
        }

    }

    public void UndoHoverFunction(bool active) {
        if (active) {
            UndoHover.SetActive(true);
        }
        else {
            UndoHover.SetActive(false);
        }
    }

    #endregion

    private Text getTextChild(GameObject GO) {
        //GameObject temp = GO.transform.GetChild(0).gameObject;
        return (GO.transform.GetChild(0).gameObject.GetComponent<Text>());
    }

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			PauseMenuReference.SetActive (true);
			Time.timeScale = 0;
		}
	}

	public void UnPause(){
		PauseMenuReference.SetActive (false);
		Time.timeScale = 1;
	}

    public void OnQuitWave()
    {
        WaveManager.S.AbortWave();
    }
}
