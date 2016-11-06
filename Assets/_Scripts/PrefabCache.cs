using UnityEngine;
using System.Collections;

public class PrefabCache : MonoBehaviour
{
    public GameObject hitTextPrefab;
	public GameObject hitTextPrefabYellow;

    void Awake()
    {
        HitText.Prefab = hitTextPrefab;
        HitText.PrefabYellow = hitTextPrefabYellow;
    }
}
