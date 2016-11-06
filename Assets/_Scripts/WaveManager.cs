using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager S;

    public int numberOfWaves;
    public int currentWaveNumber = 1;

    private Spawn spawn;

    private List<GameObject> currentUnits = new List<GameObject>();
    private int expectCount = 0;

    public int WaveNumber { get; private set; }
    public bool IsWaveRunning { get; private set; }

    void Awake()
    {
        S = this;
        this.WaveNumber = 1;
        StartCoroutine(CheckForDeadRoutine());
        IsWaveRunning = false;
    }

    // Spawns units and switches to the mode where overseer powers can be used
    public void StartWave()
    {
        if (IsWaveRunning)
        {
            return;
        }

        expectCount = 0;

        UI.S.WaveTextDisplayFunction(false);
        Spawn.S.SpawnWave();
        IsWaveRunning = true;
        ResourceManager.S.OnWaveStart();
    }

    // Stops the mode with units / overseer and switches to preparing the next wave
    public void EndWave()
    {
        if (IsWaveRunning)
        {
            IsWaveRunning = false;
            currentUnits.Clear();
            UI.S.CreateUnitBarRef.SetActive(true);
            UI.S.OverseerMenu.SetActive(false);
            UI.S.sound.Stop();
            UI.S.sound.loop = true;
            UI.S.sound.clip = Resources.Load("Sounds/WaveScreen") as AudioClip;
            if (WaveNumber >= numberOfWaves)
            {
                // At this point we've lost -> just finished the last wave and still haven't won
                UI.S.DisplayPrompt(false);
                UI.S.PlaySound("GameOver");
            }
            else
            {
                UI.S.PlaySound("WaveEnd");
                UI.S.WaveTextDisplayFunction(true);
                WaveNumber++;
                UI.S.updateWaveNumber(WaveNumber);
                ResourceManager.S.OnWaveEnd();
            }
        }
    }

    // Kills all the units and stops the wave
    public void AbortWave()
    {
        if (this.expectCount > 0) // Can't abort while units are still being created
            return;

        foreach (GameObject unit in this.currentUnits)
        {
            if (unit != null)
                Destroy(unit);
        }
    }

    public void AddUnit(GameObject obj)
    {
        this.currentUnits.Add(obj);
        obj.GetComponent<UnitBase>().creationOrder = this.currentUnits.Count;
    }

    public void IsExpectingUnits(bool val)
    {
        expectCount += val ? 1 : -1;
    }

    private IEnumerator CheckForDeadRoutine()
    {
        while (true)
        {
            if (!IsWaveRunning || expectCount > 0)
            {
                yield return new WaitForSeconds(2);
                continue;
            }

            if (currentUnits.TrueForAll(g => g == null))
            {
                yield return new WaitForSeconds(2);
                EndWave();
            }
            else
                yield return new WaitForSeconds(0.5f);
        }
    }
}
