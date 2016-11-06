using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TownGates : MonoBehaviour {

    public static TownGates S;

    public Node             TownCenterNode;
    public Sprite           freeTownCenter;
    public SpriteRenderer   sr;
    public List<Node>       NodesConnectingToCenter;      // The nodes connecting to the Town Center Node
    public int              numOfHousesToVisit;           // How many houses need to be visited in order for the gates to open (preferably the number of houses in the map)
    public int              unitsRequiredToPass;          // The number of units that need to pass in order to win
    public int              numOfHousesVisited = 0;
    public int              unitsPassed = 0;                  // The current number of units that have passed through the town square

    void Awake() {
        S = this;
        sr = GetComponent<SpriteRenderer>();
    }

    void Start() {
        numOfHousesVisited = 0;
        if (numOfHousesToVisit <= 0)
        {
            // Don't have gates to start
            sr.sprite = freeTownCenter;
			RevertNodeArrows();
        }
		else if (NodesConnectingToCenter.Count > 0) {
			DisableNodeArrows ();
		}
    }

    public void UpdateNumHousesVisited() {
        ++numOfHousesVisited;
        UI.S.UpdateUIHouses(numOfHousesVisited);
        if (numOfHousesVisited == numOfHousesToVisit) {
            DestroyTownGates();
        }
        
    }

    void DestroyTownGates() {
        sr.sprite = freeTownCenter;
        RevertNodeArrows();
        UI.S.GatesTextDisplayFunction();
        UI.S.PlaySound("ReachTownSquare");
    }

    void DisableNodeArrows() {
        for (int i = 0; i < NodesConnectingToCenter.Count; ++i) {
            Node source = NodesConnectingToCenter[i];
            RemoveAdjacentNode(ref source, TownCenterNode);
        }
    }

    void RemoveAdjacentNode(ref Node source, Node objectToRemove) {
        // Convert to list for removal
        List<Node> nodeList = new List<Node>(source.adjacentNodes);
        for (int i = 0; i < nodeList.Count; ++i) {
            Node curr = nodeList[i];
            if (curr == objectToRemove) {
                nodeList.Remove(curr);
                break;
            }
        }
        source.adjacentNodes = nodeList.ToArray();
    }

    void RevertNodeArrows() {
        for (int i = 0; i < NodesConnectingToCenter.Count; ++i) {
            Node source = NodesConnectingToCenter[i];
            AddAdjacentNode(ref source, TownCenterNode);
        }
    }

    void AddAdjacentNode(ref Node source, Node objectToAdd) {
        List<Node> nodeList = new List<Node>(source.adjacentNodes);
        nodeList.Add(objectToAdd);
        source.adjacentNodes = nodeList.ToArray();
    }
}
