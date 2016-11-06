using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class HitText : MonoBehaviour
{
    public static GameObject Prefab;
    public static GameObject PrefabYellow;
    private static GameObject anchor;


    public Text text;

    public static void Create(Vector3 position, int amount)
    {
        if (Prefab == null)
            throw new NullReferenceException("No PrefabCache script is attached to the main camera! Cannot create hit text.");
        CreateInternal(Prefab, position, amount.ToString());
    }

    public static void CreateYellow(Vector3 position, int amount)
    {
        if (PrefabYellow == null)
            throw new NullReferenceException("No PrefabCache script is attached to the main camera! Cannot create hit text.");

        CreateInternal(PrefabYellow, position, "+" + amount.ToString());
    }

    private static void CreateInternal(GameObject prefab, Vector3 position, string val)
    {
        if (anchor == null)
        {
            anchor = new GameObject("HitTextAnchor");
        }

        GameObject hitText = Instantiate(prefab);
        position.z = -2;
        hitText.transform.position = position;
        hitText.transform.SetParent(anchor.transform);

        HitText script = hitText.GetComponentInChildren<HitText>();
        script.text.text = val;
    }

    public void OnAnimationEnd()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
