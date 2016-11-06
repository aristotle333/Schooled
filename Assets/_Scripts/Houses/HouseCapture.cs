using UnityEngine;
using System.Collections;

public class HouseCapture : MonoBehaviour {

    private GameObject HouseReference;
    private bool houseTriggered = false;

    void Start() {
        HouseReference = this.transform.parent.gameObject;
    }

    void OnTriggerEnter(Collider col) {
        // Modify this accordingly, maybe special units can liberate houses
        if (col.tag == "Unit") {
            if (!houseTriggered) {
                houseTriggered = true;
                // Call spawn units coroutine
                HouseReference.GetComponent<House>().SpawnUnits();
                HouseReference.GetComponent<House>().playCaptureSound();
                HouseReference.GetComponent<House>().enableFlag();

                // Signal that a house is captured to the TownGates Manager
                TownGates.S.UpdateNumHousesVisited();

                // Collider is no longer needed
                Destroy(this.gameObject);
            }
        }
    }

}
