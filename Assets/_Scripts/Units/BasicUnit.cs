using UnityEngine;
using System.Collections;
using System;

public class BasicUnit : MonoBehaviour
{
    private UnitBase baseScript;

    void Awake()
    {
        baseScript = this.GetComponent<UnitBase>();
        baseScript.Attack += this.OnAttack;
    }

    void OnCollisionEnter(Collision coll)
    {
        baseScript.TargetEnemy(coll.gameObject);
        baseScript.PauseMove();
    }

    private void OnAttack(GameObject target)
    {
        IDamageable targetScript = target.GetComponent<IDamageable>();
        targetScript.receiveDamage(this.baseScript.stats.attackDamage);
    }

}
