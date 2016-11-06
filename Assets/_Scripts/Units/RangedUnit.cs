using UnityEngine;
using System.Collections;
using System;

public class RangedUnit : MonoBehaviour
{
    public GameObject snowballPrefab;

    private UnitBase baseScript;

    private Targeting targeting;

    void Awake()
    {
        baseScript = this.GetComponentInParent<UnitBase>();
        baseScript.Attack += OnAttack;
        targeting = new Targeting();
        InvokeRepeating("AdjustTarget", 0.25f, 0.25f);
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject target;
        Retarget retarget = other.GetComponent<Retarget>();
        if (retarget != null)
            target = retarget.target;
        else
            target = other.gameObject;

        targeting.Add(target);
        GameObject toAttack = targeting.findClosestUnit(this.transform.position);
        baseScript.TargetEnemy(toAttack);
    }

    void OnTriggerExit(Collider other)
    {
        GameObject target;
        Retarget retarget = other.GetComponent<Retarget>();
        if (retarget != null)
            target = retarget.target;
        else
            target = other.gameObject;

        this.targeting.Remove(target);
    }

    private void OnAttack(GameObject target)
    {
        GameObject snowball = Instantiate(snowballPrefab);
        snowball.transform.position = this.transform.position;
        SnowBallMovement script = snowball.GetComponent<SnowBallMovement>();
        script.Target = target;
        script.Damage = baseScript.stats.attackDamage;
    }

    private void AdjustTarget()
    {
        GameObject toAttack = targeting.findClosestUnit(this.transform.position);
        if (toAttack == null)
            baseScript.UntargetEnemy();
        else
            baseScript.TargetEnemy(toAttack);
    }

}
