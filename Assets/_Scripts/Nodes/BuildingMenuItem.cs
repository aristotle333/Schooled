using UnityEngine;
using System.Collections;

public class BuildingMenuItem : MonoBehaviour {
	public BigNode node;
	public Building building;
	public GameObject instruction;
	private float instructionShowDist = 1f;

//	public void test() {
//		print ("test");
//	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (GetComponent<SpriteRenderer> ().enabled) {
			Vector3 mousePosDiff = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
			mousePosDiff = new Vector3 (mousePosDiff.x, mousePosDiff.y, 0);
			if (mousePosDiff.magnitude <= instructionShowDist) {
				instruction.GetComponent<Canvas> ().enabled = true;
			} else {
				instruction.GetComponent<Canvas> ().enabled = false;
			}
		} else {
			instruction.GetComponent<Canvas> ().enabled = false;
		}
	}

	void OnMouseDown () {
		node.ConstructBuilding (building);
		instruction.GetComponent<Canvas> ().enabled = false;
		transform.parent.GetComponent<BuildingMenu> ().Hide ();
	}
}
 