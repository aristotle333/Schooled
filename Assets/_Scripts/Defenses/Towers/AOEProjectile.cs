using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AOEProjectile : MonoBehaviour {

    public float u;
    public List<Vector3> points;
    public GameObject targetObject;

    public bool hitFirstTarget = false;             // This is set to true when the bomb hits it's first target.

    [Header("Update the following as desired")]

    public int          damage;
    public float        AOERadius = 10f;                  // The radius of damage dealt
    public int          framesToReachAOERadius = 50;
    public float        secondsToReachTarget = 1f;
    public float        pow = 1;
    public GameObject   particle;

    private float           birthtime;
    private SphereCollider  sphereCollRef;
    private Renderer    rendRef;
    private int             currFrames = 0;
    private float           radiusIncrement;

    void Start() {
        birthtime = Time.time;
        sphereCollRef = this.GetComponent<SphereCollider>();
        rendRef = this.GetComponent<Renderer>();
        radiusIncrement = (AOERadius - sphereCollRef.radius) / (float)framesToReachAOERadius;
    }

    void Update() {
        if (targetObject == null) {
            Destroy(this.gameObject);
        }

        if (!hitFirstTarget) {
            u = Time.time - birthtime;
            u = u / secondsToReachTarget;
            u = Mathf.Pow(u, pow);
            if (targetObject != null) {
                points[points.Count - 1] = targetObject.transform.position;
            }
            Vector3 p = Interp(u, points);
            this.transform.position = p;
        }
        else {
            ++currFrames;
            sphereCollRef.radius += radiusIncrement;
            if (currFrames >= framesToReachAOERadius) {
                Destroy(this.gameObject);
            }
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
        if (coll.tag == "Unit") {
            // Add code to deal damage to unit
            IDamageable unitScript = coll.GetComponent<IDamageable>();
            unitScript.receiveDamage(damage);
            if (!hitFirstTarget) {
                hitFirstTarget = true;
                GameObject explosion = Instantiate(particle, this.transform.position, this.transform.rotation) as GameObject;
                explosion.GetComponent<explosionAOE>().PlayExplosionSound();
                rendRef.enabled = false;
            }
        }
    }
}
