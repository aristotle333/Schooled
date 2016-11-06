using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Targeting
{
    private List<GameObject> unitsInRange = new List<GameObject>();

    public void Add(GameObject obj)
    {
        unitsInRange.Add(obj);
    }

    public void Remove(GameObject obj)
    {
        unitsInRange.Remove(obj);
    }

    // Linear Search (Not very efficient, can be improved later on if game runs slow)
    public GameObject findClosestUnit(Vector3 pointClosestTo) {
        if (unitsInRange.Count <= 0)
            return null;

        float minDist = float.PositiveInfinity;     // Note that this represents squared distance
        GameObject result = unitsInRange[0];        // Defaults to the 1st gameObject in the list. There should always be at least one unit in order for this function to run

        for (int i = 0; i < unitsInRange.Count; i++)
        {
            GameObject GO = unitsInRange[i];
            if (GO == null)
            {
                unitsInRange.RemoveAt(i);
                i--;
                continue;
            }

            float squared_distance = (pointClosestTo - GO.transform.position).sqrMagnitude;
            if (minDist > squared_distance) {
                minDist = squared_distance;
                result = GO;
            }
        }

        return result;
    }

}
