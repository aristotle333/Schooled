using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingMenu : MonoBehaviour {
	static List<BuildingMenu> allMenus = new List<BuildingMenu>();
	public BuildingMenuItem[] menuItemList;
	private List<BuildingMenuItem> menuItemInstanceList = new List<BuildingMenuItem>();
	public BigNode node;
	// Use this for initialization
	void Start () {
		allMenus.Add (this);
		Vector3 pos = new Vector3 (0, 1, 0);
		float rotation = 0;
		foreach (BuildingMenuItem menuItem in menuItemList) {
			BuildingMenuItem newItem = Instantiate (menuItem, Vector3.zero, Quaternion.identity) as BuildingMenuItem;
			newItem.node = node;
			newItem.transform.parent = transform;
			newItem.transform.localPosition = Quaternion.Euler (0, 0, rotation) * pos;
			menuItemInstanceList.Add (newItem);
			rotation += 360f / menuItemList.Length;
		}
		Hide ();
	}
	
	// Update is called once per frame
	public void Show() {
		foreach (BuildingMenuItem menuItem in menuItemInstanceList) {
			menuItem.GetComponent<SpriteRenderer> ().enabled = true;
			menuItem.GetComponent<BoxCollider> ().enabled = true;
			menuItem.enabled = true;
		}
	}

	public void Hide() {
		foreach (BuildingMenuItem menuItem in menuItemInstanceList) {
			menuItem.GetComponent<SpriteRenderer> ().enabled = false;
			menuItem.GetComponent<BoxCollider> ().enabled = false;
			menuItem.enabled = false;
		}
	}

//	public static void HideAll () {
//		foreach (BuildingMenu menu in allMenus) {
//			menu.Hide();
//		}
//	}

}
