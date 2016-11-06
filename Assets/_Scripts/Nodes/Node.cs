using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Node : MonoBehaviour {
	public Node[] adjacentNodes;
	public int nextNodeInd;
	public GameObject indicator;
	public GameObject[] defenceStructures;
    public bool isFinish;
	public static bool interactible = true;
	protected float indicatorDistance;



	public Node GetNextNode () {
		if (adjacentNodes.Length > 0) {
			return adjacentNodes [nextNodeInd];
		} else {
			return null;
		}
	}


	// Use this for initialization
	protected virtual void Start () {
		nextNodeInd = 0;
		indicatorDistance = 1.5f;
		UpdateIndicator();
	}



	void OnMouseDown () {
        int origInd = nextNodeInd;
		if (adjacentNodes.Length != 0 && interactible) {
			nextNodeInd++;
			nextNodeInd %= adjacentNodes.Length;
			UpdateIndicator ();
            if (origInd != nextNodeInd)
            {
                UI.S.PlaySound("ArrowChange");
            }
            else
            {
                UI.S.PlaySound("Reject");
            }
		}
	}

	protected void UpdateIndicator () {
		if (adjacentNodes.Length == 0) {
			indicator.GetComponent<SpriteRenderer>().enabled = false;
		} else if(adjacentNodes[nextNodeInd] != null) {
            Vector3 normedVector = (adjacentNodes [nextNodeInd].transform.position - transform.position).normalized;
			indicator.transform.position = transform.position + normedVector * indicatorDistance;
			indicator.transform.localScale = new Vector3 (1f, 1f, 1f);
			float angle = Mathf.Rad2Deg * Mathf.Atan2 (normedVector.y, normedVector.x);
			indicator.transform.eulerAngles = new Vector3 (0, 0, angle);
		}
	}




}
