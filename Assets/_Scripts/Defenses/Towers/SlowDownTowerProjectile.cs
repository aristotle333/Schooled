using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlowDownTowerProjectile : MonoBehaviour {

    public float u;
    public List<Vector3> points;
    public GameObject targetObject;

    [Header("Update the following as desired")]

    public GameObject poisonParticleEffect;
    public float      verticalPoisonOffset;             // The vertical offset of the poison particle effect relative to the poisoned unit

    public int damage;
    public float slowDownAmount;
    public float slowDownPeriod;

    public float secondsToReachTarget = 1f;
    public float pow = 1;
    //public float sinStrength = 0.2f;

    private float birthtime;

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
        this.transform.position = p;
        if (u > 1.2f * secondsToReachTarget) {
            Destroy(this.gameObject);
        }

        Vector3 currentRot = this.transform.eulerAngles;
        currentRot.z += 100 * Time.deltaTime;
        this.transform.eulerAngles = currentRot;
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
        if (coll.tag == "Unit" && coll.gameObject == targetObject) {
            // Add code to deal damage to unit
            IDamageable unitScript = coll.GetComponent<IDamageable>();
            unitScript.receiveDamage(damage);

            coll.gameObject.GetComponent<UnitBase>().changeSpeed(-slowDownAmount, slowDownPeriod);

            // Creating a poisoned particle effect
            GameObject particleEffect = Instantiate(poisonParticleEffect) as GameObject;
            particleEffect.name = "PoisonParticle";
            particleEffect.transform.SetParent(coll.gameObject.transform);
            Vector3 position = new Vector3(0, verticalPoisonOffset, 0);
            particleEffect.transform.localPosition = position;
            Destroy(this.gameObject);
        }
    }
}
