using UnityEngine;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
public class NodeConnect : MonoBehaviour
{
    private static GameObject pathAnchor;

    public GameObject pathPrefab;
    List<GameObject> toDest = new List<GameObject>();
    List<Node> nodes = new List<Node>();
    Dictionary<int, HashSet<int>> roads = new Dictionary<int, HashSet<int>>();
    Dictionary<Node, int> nodeIds = new Dictionary<Node, int>();

    void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            roads.Clear();
            FindNodes();
            RemoveOld();

            for(int i = 0; i < nodes.Count; ++i)
            {
                Node ni = nodes[i];
                for(int k = 0; k < ni.adjacentNodes.Length; ++k)
                {
                    Node nj = ni.adjacentNodes[k];
                    if (nj == null)
                        continue;

                    int j = nodeIds[nj];

                    HashSet<int> curSet;
                    if (roads.TryGetValue(Math.Min(i, j), out curSet) && curSet.Contains(Math.Max(i, j)))
                    {
                        continue;
                    }
                    else if (!roads.TryGetValue(Math.Min(i, j), out curSet))
                    {
                        roads.Add(Math.Min(i, j), new HashSet<int>());
                    }
                    roads[Math.Min(i, j)].Add(Math.Max(i, j));
                    Vector3 relPos = nj.transform.position - ni.transform.position;
                    GameObject newPath = CreatePath(i, relPos);
                    toDest.Add(newPath);
                }
            }
        }
    }

    private void FindNodes()
    {
        nodes.Clear();
        foreach (Transform node in transform)
        {
            if (transform != node)
            {
                Node n = node.GetComponent<Node>();
                if (n != null)
                    nodes.Add(n);
            }
        }

        nodeIds.Clear();
        for (int i = 0; i < nodes.Count; i++)
        {
            nodeIds[nodes[i]] = i;
        }
    }

    private void RemoveOld()
    {
        foreach (GameObject dest in toDest)
        {
            DestroyImmediate(dest); 
        }
        toDest.Clear();

        if (pathAnchor != null) {
            DestroyImmediate(pathAnchor);
        }
    }

    private GameObject CreatePath(int i, Vector3 relPos)
    {
        if (pathAnchor == null)
        {
            pathAnchor = GameObject.Find("PathAnchor");
            if (pathAnchor == null)
                pathAnchor = new GameObject("PathAnchor");
        }

        GameObject newPath = Instantiate(pathPrefab, nodes[i].transform.position, Quaternion.LookRotation(relPos)) as GameObject;
        newPath.transform.SetParent(pathAnchor.transform);

        Vector3 rotVec = (relPos).normalized;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(rotVec.y, rotVec.x);
        newPath.transform.eulerAngles = new Vector3(0, 0, angle - 90f);
        newPath.transform.localScale = new Vector3(.1f, relPos.magnitude / 7.25f, 1);
        return newPath;
    }
}
