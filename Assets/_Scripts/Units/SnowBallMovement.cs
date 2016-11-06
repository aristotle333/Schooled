using UnityEngine;
using System;
using System.Collections;

public class SnowBallMovement : MonoBehaviour
{
    public float curvePercent;
    public float projectileTime;

    public GameObject Target { get; set; }
    public int Damage { get; set; }

    [Header("Debugging:")]
    [SerializeField]
    private Vector3 pt1;
    [SerializeField]
    private Vector3 pt2;
    [SerializeField]
    private Vector3 pt3;

    private float startTime;

    void Start()
    {
        if (Target == null)
        {
            Destroy(this.gameObject);
            throw new NullReferenceException("Snowball was not given a target.");
        }

        pt1 = this.transform.position;
        Vector3 pt3 = Target.transform.position;
        pt2 = DeterminePoint2(pt1, pt3);

        startTime = Time.time;
    }

    void FixedUpdate()
    {
        float u = (Time.time - startTime) / projectileTime;
        if (u > 1)
            HitTarget();
        else
            this.transform.position = TripleLerp(u);
    }


    private Vector3 DeterminePoint2(Vector3 pt1, Vector3 pt3)
    {
        Vector3 midpoint = (pt1 + pt3) * 0.5f;
        Vector3 diff = pt3 - pt1;

        float offsetDist = diff.magnitude * this.curvePercent;

        float z = Quaternion.FromToRotation(Vector3.right, diff).eulerAngles.z;
        float perpendicularZ = (z > 90 && z < 270) ? z - 90 : z + 90;
        Vector3 perpDir = Quaternion.Euler(0, 0, perpendicularZ) * Vector3.right;

        return midpoint + (perpDir * offsetDist);
    }

    private Vector3 TripleLerp(float u)
    {
        if (Target != null)
            this.pt3 = Target.transform.position;

        Vector3 pt12 = Vector3.Lerp(pt1, pt2, u);
        Vector3 pt23 = Vector3.Lerp(pt2, pt3, u);
        Vector3 pt123 = Vector3.Lerp(pt12, pt23, u);

        return pt123;
    }

    private void HitTarget()
    {
        if (!this.isActiveAndEnabled)
            return;

        if (Target != null)
        {
            Retarget retarget = Target.GetComponent<Retarget>();
            GameObject actualTarget = (retarget == null) ? Target : retarget.damageable;

            IDamageable targetScript = actualTarget.GetComponent<IDamageable>();
            if (targetScript == null)
                targetScript = actualTarget.GetComponentInParent<IDamageable>();

            targetScript.receiveDamage(this.Damage);
        }
        Destroy(this.gameObject);
    }
}
