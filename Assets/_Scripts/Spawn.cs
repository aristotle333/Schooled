using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public enum UnitType { UnitBasic, Archer, Engineer, Balloon }

[Serializable]
public class SpawnPair {
    public UnitType type;
    public GameObject prefab;
}

public class Spawn : MonoBehaviour {
    public GameObject spawnPoint;
    public static Spawn S;
    public Node firstNode;

    public float GenerateResourceRate = 0.5f;
    public float spawnUnitsInterval;

    public List<SpawnPair> spawnTypes;

    private Dictionary<UnitType, GameObject> prefabMapping;
    private List<UnitType> toSpawn = new List<UnitType>();
    private Coroutine spawnLoop;

    public IEnumerable<UnitType> ToSpawn { get { return toSpawn; } }

    void Awake()
    {
        S = this;
        prefabMapping = spawnTypes.ToDictionary(p => p.type, p => p.prefab);
    }

	void Start ()
    {
        spawnPoint = transform.FindChild("Spawn Point").gameObject;
        //InvokeRepeating("GenerateResource", 0, GenerateResourceRate);
    }

    void GenerateResource() {
        UI.S.changeResources(1);
    }


    public void SpawnUnit(UnitType type)
    {
        if (spawnLoop != null) // Currently spawning so the spawn list can't be changed
            return;

        toSpawn.Add(type);
    }

    public void UndoSpawn()
    {
        if (toSpawn.Count > 0)
        {
            UnitType type = toSpawn[toSpawn.Count - 1];
            toSpawn.RemoveAt(toSpawn.Count - 1);
            ResourceManager.S.AddResources(ResourceManager.UnitCosts[type]);
        }
    }

    public void SpawnWave()
    {
        this.spawnLoop = StartCoroutine(SpawningRoutine());
    }

    private IEnumerator SpawningRoutine()
    {
        WaveManager.S.IsExpectingUnits(true);

        foreach (UnitType type in toSpawn)
        {
            yield return new WaitForSeconds(spawnUnitsInterval);

            GameObject prefab = prefabMapping[type];
            GameObject newUnit = Instantiate(prefab, spawnPoint.transform.position, Quaternion.identity) as GameObject;
            newUnit.GetComponent<UnitBase>().firstTargetNode = firstNode;

            WaveManager.S.AddUnit(newUnit);
        }

        WaveManager.S.IsExpectingUnits(false);
        toSpawn.Clear();
        spawnLoop = null;
    }
}
