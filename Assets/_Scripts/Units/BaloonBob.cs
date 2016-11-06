using UnityEngine;
using System.Collections;

public class BaloonBob : MonoBehaviour
{
    public float frequency;
    public float amplitude;

	// Update is called once per frame
	void FixedUpdate ()
    {
        float angle = frequency * Time.time * 2 * Mathf.PI;
        float yoff = amplitude * Mathf.Sin(angle);

        Vector3 pos = this.transform.position;
        pos.y += yoff;
        this.transform.position = pos;
	}
}
