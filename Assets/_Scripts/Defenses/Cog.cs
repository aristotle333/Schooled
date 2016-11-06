using UnityEngine;
using System.Collections;

public class Cog : MonoBehaviour {
    public GameObject connectedWaterTower;

    void Start() {
        connectedWaterTower = this.transform.parent.gameObject;
    }

    void OnTriggerEnter(Collider col) {
        UnitBase baseScript = col.GetComponent<UnitBase>();

        if (baseScript == null)
            return;

        if (baseScript.type == UnitType.Engineer) {
            connectedWaterTower.GetComponent<FloodControl>().FloodDisabled();

            // TODO: decide if we want the engineers to be used up by this
            Destroy(col.gameObject);

            // Destroy itself (Cogs are no longer needed)
            Destroy(this.gameObject);
            connectedWaterTower.GetComponent<FloodControl>().PlayCollectCogSound();
        }
    }

}
