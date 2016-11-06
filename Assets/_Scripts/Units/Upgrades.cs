using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// This is a static class because everything it does needs to persist between scenes
public static class Upgrades
{
    private static Dictionary<UnitType, int> unitLevel;

    public static void Reset()
    {
        if (unitLevel == null)
            FirstTimeInit();
        foreach (UnitType type in unitLevel.Keys)
        {
            unitLevel[type] = 0;
        }
    }

    public static int GetLevel(UnitType type)
    {
        if (unitLevel == null)
            FirstTimeInit();
        return unitLevel[type];
    }

    public static void Upgrade(UnitType type)
    {
        if (unitLevel == null)
            FirstTimeInit();
        unitLevel[type]++;
    }

    private static void FirstTimeInit()
    {
        unitLevel = new Dictionary<UnitType, int>();
        var allTypes = Enum.GetValues(typeof(UnitType)).Cast<UnitType>();
        // Create a dictionary with all types, and set the level of all of them to zero.
        unitLevel = allTypes.ToDictionary(t => t, t => 0); 
    }
}
