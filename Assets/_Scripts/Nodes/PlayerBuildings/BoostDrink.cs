using UnityEngine;
using System.Collections;

public class BoostDrink : Building {
	public float speedBoost = 2f;
	public float duration = 5f;

	public GameObject BoostParticleEffect;
	public float      verticalParticleOffset; 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.tag == "Unit") {
			coll.gameObject.GetComponent<UnitBase> ().changeSpeed (speedBoost, duration);
			// Particle Effect
			GameObject particleEffect = Instantiate(BoostParticleEffect) as GameObject;
			particleEffect.name = "BoostParticle";
			particleEffect.transform.SetParent(coll.gameObject.transform);
			Vector3 position = new Vector3(0, verticalParticleOffset, 0);
			particleEffect.transform.localPosition = position;
		}
	}
}
