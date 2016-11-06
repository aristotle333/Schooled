using UnityEngine;
using System.Collections;

public class CandyFactory : Building {
	public static int generatingRate = 20;
//	public static float generatingInterval = 1f;
	public static int numOfCandyFactory = 0;
//	float cooldown;

	// Use this for initialization
	void Start () {
		numOfCandyFactory++;
//		cooldown = generatingInterval;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//		cooldown -= Time.deltaTime;
//		if (cooldown <= 0) {
//			cooldown += generatingInterval;
//			GenerateCandy ();
//		}
	}

//	void GenerateCandy () {
//        UI.S.changeResources(generatingRate);
//		print (generatingRate + " candy generated.");
//	}

}
