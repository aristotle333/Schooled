using UnityEngine;
using System.Collections;

public class Barricade : MonoBehaviour, IDamageable {

    public int              health;
    public float            healthUpdateRate;
    public Sprite[]         healthTextures = null;
    public GameObject       healthBar;


    private int             startingHealth;

    public int Health { get { return health; } }
    public int MaxHealth { get { return startingHealth; } }

    void Start() {
        healthBar = this.transform.Find("Health Bar").gameObject;
        startingHealth = health;

        // InvokeRepeating Methods for Tower
        InvokeRepeating("updateHealthBar", 0, healthUpdateRate);

        //This is super hacky, but we don't want sideways barricades anymore
        this.transform.rotation = Quaternion.identity;
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


    // This function should be called by whoever deals damage to the barricade
    public void receiveDamage(int amount)
    {
        UI.S.PlaySound("Hit");
        health -= amount;
        HitText.Create(this.transform.position, amount);
        checkHealth();
    }

    public void checkHealth() {
        if (health <= 0) {
            Destroy(this.gameObject);
        }
    }

}
