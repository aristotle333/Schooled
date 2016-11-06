using UnityEngine;
using System.Collections;

public class TunnelExit : MonoBehaviour {
	public static TunnelExit instance;
	public Node node;
	public GameObject[] Rocks;

	public void activate () {
		foreach (GameObject rock in Rocks) {
			rock.GetComponent<SpriteRenderer> ().enabled = false;
		}
	}
	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
		} else {
			print("ERROR: TunnelExit class is singelton");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
