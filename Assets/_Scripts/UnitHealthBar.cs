using UnityEngine;
using System.Collections;

public class UnitHealthBar : MonoBehaviour {

    public Sprite[] healthTextures = null;

    public float UpdateHealthRate = 0.1f;

    private GameObject parent;

    private int health;
    private int startingHealth;

    private IDamageable parentScript;

    // Use this for initialization
    void Start () {
        parentScript = this.transform.parent.GetComponent<IDamageable>();
        health = parentScript.Health;
        startingHealth = parentScript.MaxHealth;
        InvokeRepeating("updateHealthBar", 0, UpdateHealthRate);
    }

    public void updateHealthBar() {
        if(parentScript != null)
        {
            health = parentScript.Health;
        }
        float healthFraction = (float)health / (float)startingHealth;
        this.transform.localScale = new Vector3(healthFraction * 0.5f, 1, 1);

        if (healthFraction <= 1f) {
            this.GetComponent<SpriteRenderer>().sprite = healthTextures[0];
        }
        if (healthFraction < 0.6f) {
            this.GetComponent<SpriteRenderer>().sprite = healthTextures[1];
        }
        if (healthFraction < 0.2f) {
            this.GetComponent<SpriteRenderer>().sprite = healthTextures[2];
        }
    }
}
