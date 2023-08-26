using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuikGraph;

public class Simple_graph_tester : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        string test_string = PGL.Version();
        Debug.Log(test_string);

        // Create an example graph
        // first create three different vertices 
        thingy thingy1 = new thingy("A", "Vertex A");
        thingy thingy2 = new thingy("B", "Vertex B");
        thingy thingy3 = new thingy("C", "Vertex C");

        PGL.GraphDrawer<thingy> graph = new PGL.GraphDrawer<thingy>(10);

        // Add these to the graph 
        graph.AddVertex(thingy1);
        graph.AddVertex(thingy2);
        graph.AddVertex(thingy3);

        // Add the edges now 
        graph.AddEdge(new Edge<thingy>(thingy1, thingy2), 1);
        graph.AddEdge(new Edge<thingy>(thingy2, thingy3), 1);
        graph.AddEdge(new Edge<thingy>(thingy3, thingy1), 1);

        graph.GetMaps();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public class thingy
{
    public string name;
    public string description;

    public thingy(string name, string description)
    {
        this.name = name;
        this.description = description;
    }
}