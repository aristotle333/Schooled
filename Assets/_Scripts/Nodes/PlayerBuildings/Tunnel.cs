using UnityEngine;
using System.Collections;

public class Tunnel : Building {

	// Use this for initialization
	void Start () {
		TunnelExit.instance.activate ();
	}

	// Update is called once per frame
	void FixedUpdate () {

	}

	void OnTriggerEnter(Collider coll) {
		if (TunnelExit.instance != null) {
			if (coll.tag == "Unit") {
				coll.gameObject.GetComponent<UnitBase> ().PauseMove ();
				coll.gameObject.GetComponent<UnitBase> ().PortalTo (TunnelExit.instance.node);
				coll.gameObject.GetComponent<UnitBase> ().StartMove (TunnelExit.instance.node);
			}
		}
	}
}
