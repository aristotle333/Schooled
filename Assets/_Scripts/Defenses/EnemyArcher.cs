using UnityEngine;
using System.Collections;
using System;

public class EnemyArcher : MonoBehaviour, IDamageable
{
    public int maxHealth;
    public int damage;
    public float attackRate;
    public GameObject projectilePrefab;

    private Targeting targeting = new Targeting();

    public int Health { get; private set; }

    public int MaxHealth { get { return maxHealth; } }

    void Awake()
    {
        this.Health = maxHealth;
        InvokeRepeating("Attack", UnityEngine.Random.value, attackRate);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Unit"))
            targeting.Add(col.gameObject);
    }

    public void receiveDamage(int amount)
    {
        Health -= amount;
        HitText.Create(this.transform.position, amount);
        if (Health <= 0)
            Destroy(this.gameObject);
    }

    void Attack()
    {
        GameObject target = targeting.findClosestUnit(this.transform.position);
        if (target == null)
            return;

        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = this.transform.position;

        SnowBallMovement projScript = projectile.GetComponent<SnowBallMovement>();
        projScript.Target = target;
        projScript.Damage = this.damage;
    }
}
