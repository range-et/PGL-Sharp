using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuikGraph;
using TreeEditor;

public class TestScript : MonoBehaviour
{
    public int TestProperty;
    public AdjacencyGraph<int, Edge<int>> TestGraph;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("test log");

        TestGraph = new AdjacencyGraph<int, Edge<int>>();
        TestGraph.AddVertex(1);
        TestGraph.AddVertex(2);
        TestGraph.AddVertex(3);
        TestGraph.AddEdge(new Edge<int>(1, 2));
        TestGraph.AddEdge(new Edge<int>(3, 1));
        TestGraph.AddEdge(new Edge<int>(3, 2));

        Debug.Log("_____________________");
        foreach (var edge in TestGraph.Edges)
        {
            Debug.Log(edge.Source + "->" + edge.Target);
        }

        Debug.Log("_____________________");
        foreach (var node in TestGraph.Vertices)
        {
            Debug.Log(node);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}