using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager S;

    public static Dictionary<UnitType, int> UnitCosts = new Dictionary<UnitType, int>
    {
        { UnitType.UnitBasic, 5 },
        { UnitType.Archer, 10 },
        { UnitType.Balloon, 20 },
        { UnitType.Engineer, 30 },
    };

    public int resourcesPerWave;

    private int _resources;
    public int Resources
    {
        get { return _resources; }
        private set
        {
            _resources = value;
            UI.S.updateResourceValue(this._resources);
        }
    }

    void Awake()
    {
        S = this;
    }

    void Start()
    {
        Resources = resourcesPerWave;
    }

    public void AddResources(int amount)
    {
        this.Resources += amount;
    }


    public bool TryDoPurchase(int amount)
    {
        if (Resources < amount)
            return false;

        Resources -= amount;
        return true;
    }

    public bool TryDoPurchase(UnitType unit)
    {
        return TryDoPurchase(UnitCosts[unit]);
    }

    public void OnWaveStart()
    {
        Resources = 0;
    }

    public void OnWaveEnd()
    {
        Resources += resourcesPerWave;
		Resources += CandyFactory.numOfCandyFactory * CandyFactory.generatingRate;
    }
}
