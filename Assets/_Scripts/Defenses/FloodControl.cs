using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EffectedNode {
    public Node                 NodeEffected;                   // The nodes directly effected by the flood
    public List<Node>           ParentsOfEffectedNode;        // The parents of the effected nodes
}

public class FloodControl : MonoBehaviour {

    public List<EffectedNode> EffectedNodes;          // The nodes affected by the flood
    public List<GameObject> EffectedPaths;          // Modify this later to do it programatically

    public Color waterColor;             // The color of the water/flood

    public bool towerIsActive = true;   // Set to true if water tower is initially active to create a flood

    void Start() {
        if (towerIsActive) {
            DisableNodeArrows();
            DrawWater();
        }
    }

    // This function should be called once, whenever the flood is disabled
    public void FloodDisabled() {
        RevertNodeArrows();
        DestroyWaterTiles();
    }

    void DisableNodeArrows() {
        for (int i = 0; i < EffectedNodes.Count; ++i) {
            Node target = EffectedNodes[i].NodeEffected;
            for (int j = 0; j < EffectedNodes[i].ParentsOfEffectedNode.Count; j++) {
                Node source = EffectedNodes[i].ParentsOfEffectedNode[j];
                RemoveAdjacentNode(ref source, target);
            }
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

    void DrawWater() {
        GameObject FloodAnchor = new GameObject();
        FloodAnchor.name = "FloodAnchor";
        for (int i = 0; i < EffectedPaths.Count; i++) {
            GameObject floodedPath = Instantiate(EffectedPaths[i], EffectedPaths[i].transform.position, EffectedPaths[i].transform.rotation) as GameObject;
            floodedPath.transform.SetParent(FloodAnchor.transform);
            floodedPath.GetComponent<SpriteRenderer>().color = waterColor;
        }
    }

    // Call this when flood has been disabled to revert the nodes and their arrows
    void RevertNodeArrows() {
        for (int i = 0; i < EffectedNodes.Count; ++i) {
            Node target = EffectedNodes[i].NodeEffected;
            for (int j = 0; j < EffectedNodes[i].ParentsOfEffectedNode.Count; j++) {
                Node source = EffectedNodes[i].ParentsOfEffectedNode[j];
                AddAdjacentNode(ref source, target);
            }
        }
    }

    void AddAdjacentNode(ref Node source, Node objectToAdd) {
        List<Node> nodeList = new List<Node>(source.adjacentNodes);
        nodeList.Add(objectToAdd);
        source.adjacentNodes = nodeList.ToArray();
    }

    void DestroyWaterTiles() {
        Destroy(GameObject.Find("FloodAnchor"));
    }

    public void PlayCollectCogSound() {
        this.GetComponent<AudioSource>().Play();
    }

}
