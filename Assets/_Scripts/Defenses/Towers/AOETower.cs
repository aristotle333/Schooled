using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AOETower : MonoBehaviour, IDamageable {

    public int health;
    public float fireRate;
    public float healthUpdateRate;       // how often the health bar updates

    public GameObject projectilePrefab;
    public Sprite[] healthTextures = null;

    public float timeToRebuild;

    private GameObject shootingLocation;
    private GameObject healthBar;
    private Vector3 shootingLocationVector;
    private int startingHealth;

    private Targeting targeting = new Targeting();

    public int Health { get { return health; } }
    public int MaxHealth { get { return startingHealth; } }

    void Start() {
        shootingLocation = this.transform.Find("Fire Location").gameObject;
        healthBar = this.transform.Find("Health Bar").gameObject;
        shootingLocationVector = shootingLocation.transform.position;
        startingHealth = health;

        // InvokeRepeating Methods for Tower
        InvokeRepeating("shootUnit", 0, fireRate);
        InvokeRepeating("updateHealthBar", 0, healthUpdateRate);
    }

    public void shootUnit() {
        GameObject target = targeting.findClosestUnit(this.transform.position);
        if (target == null) {
            return;
        }
        // TODO: Change the sound to explosion shot
        UI.S.PlaySound("AOErelease");
        GameObject projectile = Instantiate(projectilePrefab, shootingLocation.transform.position, Quaternion.identity) as GameObject;
        projectile.GetComponent<AOEProjectile>().points.Add(shootingLocation.transform.position);
        projectile.GetComponent<AOEProjectile>().points.Add(findInterpolationVector(target.transform.position));
        projectile.GetComponent<AOEProjectile>().points.Add(target.transform.position);
        projectile.GetComponent<AOEProjectile>().targetObject = target;
    }

    public void updateHealthBar() {
        float healthFraction = (float)health / (float)startingHealth;
        healthBar.transform.localScale = new Vector3(healthFraction, 1, 1);

        if (healthFraction <= 1f) {
            healthBar.GetComponent<SpriteRenderer>().sprite = healthTextures[0];
        }
        if (healthFraction < 0.6f) {
            healthBar.GetComponent<SpriteRenderer>().sprite = healthTextures[1];
        }
        if (healthFraction < 0.2f) {
            healthBar.GetComponent<SpriteRenderer>().sprite = healthTextures[2];
        }
    }

    // Unit enters the range
    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Unit") {
            targeting.Add(coll.gameObject);
        }
    }

    // Unit is in the range
    void OnTriggerExit(Collider coll) {
        if (coll.gameObject.tag == "Unit") {
            targeting.Remove(coll.gameObject);
        }
    }

    // This function finds ONE intermediate interpolation vector location between origin and target. It will not work if we add more points
    Vector3 findInterpolationVector(Vector3 targetVec) {
        Vector3 result = shootingLocationVector;
        // Taking the tower as reference (origin)
        float dx = shootingLocationVector.x - targetVec.x;
        float dy = shootingLocationVector.y - targetVec.y;

        result.x -= 1.5f * dx;
        result.y -= 0.5f * dy;
        return result;
    }

    // This function should be called by whoever deals damage to the baricade
    public void receiveDamage(int amount) {
        health -= amount;
        HitText.Create(this.transform.position, amount);
        checkHealth();
    }

    public void checkHealth() {
        if (health <= 0) {
            health = 0;
            Destroy(this.gameObject);
        }
    }
}
