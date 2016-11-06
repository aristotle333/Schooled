using UnityEngine;
using System.Collections;

// This is a bit of a hack to make RangedUnits stop at barricades
public class StopAtBarricade : MonoBehaviour
{
    private UnitBase baseScript;

    void Awake()
    {
        baseScript = GetComponent<UnitBase>();
    }

    void OnCollisionEnter()
    {
        baseScript.PauseMove();
    }
}
