using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HouseUnits {
    public GameObject unitPrefab;
    public int        amount;

}

public class House : MonoBehaviour {
    public GameObject               spawnPoint;
    public GameObject               flag;
    public List<HouseUnits>         unitsList;
    public Node                     TargetNode;             // The node that units from this house will be redirected to
    public bool                     HouseCaptured = false;

    public float                    spawnUnitsInterval = 0.7f;

    private List<GameObject>        spawnList = new List<GameObject>();              // A randomized list of all the units to be spawn
    private Coroutine               spawnLoop;

    void Start() {
        flag.SetActive(false);
        GenerateRandomOrder();
    }

    // Generates a random spawn list for the units coming out of the house
    void GenerateRandomOrder() {
        List<GameObject> spawnListTemp = new List<GameObject>();
        for (int i = 0; i < unitsList.Count; ++i) {
            for (int j = 0; j < unitsList[i].amount; ++j) {
                spawnListTemp.Add(unitsList[i].unitPrefab);
            }
        }

        while (spawnListTemp.Count != 0) {
            int idx = (int)Random.Range(0, spawnListTemp.Count - 1);
            spawnList.Add(spawnListTemp[idx]);
            spawnListTemp.RemoveAt(idx);
        }
    }


    public void SpawnUnits() {
        this.spawnLoop = StartCoroutine(SpawningRoutine());
    }

    private IEnumerator SpawningRoutine() {
        WaveManager.S.IsExpectingUnits(true);

        foreach (GameObject unit in spawnList) {
            yield return new WaitForSeconds(spawnUnitsInterval);

            GameObject newUnit = Instantiate(unit, spawnPoint.transform.position, Quaternion.identity) as GameObject;
            newUnit.GetComponent<UnitBase>().firstTargetNode = TargetNode;
            WaveManager.S.AddUnit(newUnit);
        }

        WaveManager.S.IsExpectingUnits(false);
        spawnList.Clear();
        spawnLoop = null;
    }

    public void playCaptureSound() {
        this.GetComponent<AudioSource>().Play();
    }

    public void enableFlag() {
        flag.SetActive(true);
    }
}
