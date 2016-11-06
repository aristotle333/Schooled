using UnityEngine;
using System.Collections;

public class EngineerUnit : MonoBehaviour
{
    private UnitBase baseScript;
    private GameObject currentTrap;

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

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Trap"))
            return;

        // Do disable
        baseScript.PauseMove();
        currentTrap = other.gameObject;
        Invoke("DoDisableTrap", .3f);
    }

    private void DoDisableTrap()
    {
        // A different Engineer may have already destroyed it
        UI.S.PlaySound("AttackBoost");
        if (currentTrap != null)
            Destroy(currentTrap);
        baseScript.ContinueMove();
        currentTrap = null;
    }

    private void OnAttack(GameObject target)
    {
        IDamageable targetScript = target.GetComponent<IDamageable>();
        targetScript.receiveDamage(this.baseScript.stats.attackDamage);
    }
}
