using UnityEngine;
using System.Collections;
using System;

public class EnemyUnit : MonoBehaviour, IDamageable
{
    public int maxHealth;
    public int damage;
    public int attackRate;

    public int Health { get; private set; }
    public int MaxHealth { get { return maxHealth; } }

    private Targeting targeting;

    void Awake()
    {
        this.Health = maxHealth;
        InvokeRepeating("Attack", UnityEngine.Random.value, attackRate);
        targeting = new Targeting();
    }

    void OnCollisionEnter(Collision col)
    {
        if (!col.gameObject.CompareTag("Unit"))
            return;

        targeting.Add(col.gameObject);
    }

    public void receiveDamage(int amount)
    {
        this.Health -= amount;
        HitText.Create(this.transform.position, amount);
        if (Health <= 0)
            Destroy(this.gameObject);
    }

    private void Attack()
    {
        GameObject target = targeting.findClosestUnit(this.transform.position);
        if (target == null)
            return;

        IDamageable targetScript = target.GetComponent<IDamageable>();
        targetScript.receiveDamage(this.damage);
    }
}
