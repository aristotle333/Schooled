using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TownSquare : MonoBehaviour {

    public float timeToCapture = 5f;
    public float timeCapturing = 0f;
    public float captureTimeStart = 0f;
    public bool  isCapturing = false;

    void OnTriggerEnter(Collider coll) {
        if (coll.tag == "Unit") {
            ++TownGates.S.unitsPassed;
            UI.S.UpdateUnitsPassed(TownGates.S.unitsPassed);
            this.GetComponent<AudioSource>().Play();
            Destroy(coll.gameObject);
            //unitList.Add(coll.gameObject);
        }
    }

    //public List<GameObject> unitList;

    //void Start() {
    //    //InvokeRepeating("checkList", 0f, 0.1f);
    //}

    //void Update() {
    //    timeCapturing = Time.time - captureTimeStart;
    //    if (isCapturing && (timeCapturing > timeToCapture)) {

    //        // Pause the Game and show Winning Prompt
    //        Time.timeScale = 0f;
    //        UI.S.DisplayPrompt(true);
    //    }
    //}

    //void checkList() {
    //    if (unitList.Count == 0 || unitList == null) {
    //        isCapturing = false;
    //        return;
    //    }

    //    for (int i = 0; i < unitList.Count; i++) {
    //        if (unitList[i] == null) {
    //            unitList.RemoveAt(i);
    //            i--;
    //            continue;
    //        }
    //    }
    //    if (unitList.Count == 0) {
    //        isCapturing = false;
    //    }
    //    else {
    //        // Check if it is the first time capturing
    //        if (!isCapturing) {
    //            captureTimeStart = Time.time;
    //            isCapturing = true;
    //        }
    //        else {
    //            isCapturing = true;
    //        }
    //    }
    //}

}
