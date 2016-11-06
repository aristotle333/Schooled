using UnityEngine;
using System.Collections;

[System.Serializable]
public class BigNode : Node {
	public Building building;
	private bool _allowBuild;
	private const float menuSize = 3f;
	public bool allowBuild {
		get { return _allowBuild; }
		set { 
			_allowBuild = value;
			if (value) {
				GetComponent<SpriteRenderer> ().color = Color.white;
			} else {
				GetComponent<SpriteRenderer> ().color = Color.red;
			}
		}
	}
	public BuildingMenu buildingMenu;

	private bool visited;

	public void ConstructBuilding (Building _building) {
		building = Instantiate (_building);
		building.transform.parent = transform;
		building.transform.localPosition = Vector3.zero;
	}
	// Use this for initialization
	protected override void Start () {
		allowBuild = false;
		visited = false;
		base.Start ();
		indicatorDistance = 2.5f;
		UpdateIndicator ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0) || Input.GetMouseButton (1)) {
			Vector3 mousePosDiff = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
			mousePosDiff = new Vector3 (mousePosDiff.x, mousePosDiff.y, 0);
			if (mousePosDiff.magnitude > menuSize) {
				buildingMenu.Hide ();
			}
		}

		// check if defence structures are destroyed
		bool defenceDestroyed = true;
		foreach (GameObject defenceStruct in defenceStructures) {
			if (defenceStruct != null) {
				defenceDestroyed = false;
			}
		}
		if (defenceDestroyed && visited) {
			allowBuild = true;
		}
	}

	void OnMouseOver () {
		if (Input.GetMouseButtonDown (1)) {
			if (building == null && allowBuild == true) {
				buildingMenu.Show ();
			}
		}
	}


	void OnTriggerEnter(Collider coll) {
		if (coll.tag == "Unit") {
			visited = true;
		}
	}
}
