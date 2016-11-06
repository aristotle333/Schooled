using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour {

    public int damage;

    // Unit enters the spike
    void OnTriggerEnter(Collider coll) {
        // Modify this when we add engineers
        if (coll.gameObject.tag == "SpikeDefuser") {
            // TODO: play Engineer defuse spike sound
            Destroy(this.gameObject);
        }

        if (coll.gameObject.tag == "Unit") {
            this.GetComponent<AudioSource>().Play();
            coll.gameObject.GetComponent<UnitBase>().receiveDamage(damage);
        }
    }
}
