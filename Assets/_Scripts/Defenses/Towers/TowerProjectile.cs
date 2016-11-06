using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerProjectile : MonoBehaviour {
    public float                u;
    public int                  damage;
    public List<Vector3>        points;
    public GameObject           targetObject;

    [Header("Update the following as desired")]
    public float                secondsToReachTarget = 1f;
    public float                pow = 1;
    public float                sinStrength = 0.2f;

    private float               birthtime;

    void Start() {
        birthtime = Time.time;
    }

    void Update() {
        if (targetObject == null) {
            Destroy(this.gameObject);
        }
        u = Time.time - birthtime;
        u = u / secondsToReachTarget;
        u = Mathf.Pow(u, pow);
        //        u = 1 - Mathf.Pow(1-u, pow);
        //u = u + Mathf.Sin(2*Mathf.PI*u) * sinStrength;
        if (targetObject != null) {
            points[points.Count - 1] = targetObject.transform.position;
        }
        Vector3 p = Interp(u, points);
        transform.Rotate(0f, 0f, 4f);
        this.transform.position = p;
        if (u > 1.2f * secondsToReachTarget)
        {
            Destroy(this.gameObject);
        }
    }

    Vector3 Interp(float u, List<Vector3> pS, int i0 = 0, int i1 = -1) {
        if (i1 == -1) {
            i1 = pS.Count - 1;
        }
        if (i0 == i1) return pS[i0];
        Vector3 pL = Interp(u, pS, i0, i1 - 1);
        Vector3 pR = Interp(u, pS, i0 + 1, i1);
        Vector3 pLR = (1 - u) * pL + u * pR;
        return pLR;
    }

    void OnTriggerEnter(Collider coll) {
        if (!this.isActiveAndEnabled)
            return;

        if (coll.tag == "Unit") {
            // Add code to deal damage to unit
            IDamageable unitScript = coll.GetComponent<IDamageable>();
            unitScript.receiveDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
