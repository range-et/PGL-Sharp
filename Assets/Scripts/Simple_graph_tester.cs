using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuikGraph;
using Unity.VisualScripting;

public class Simple_graph_tester : MonoBehaviour
{

    [SerializeField] public GameObject playerPrefab;

    private int number = 3;
    private int counter = 0;
    List<thingy> thingys = new List<thingy> {new thingy("D", "Vertex D"),
        new thingy("E", "Vertex E"),
        new thingy("F", "Vertex F") };
    PGL.GraphDrawer<thingy> graph;
    
    
    
    
    void Awake()
    {
        graph = new PGL.GraphDrawer<thingy>(10);
    }
    

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
        
        // Add these to the graph 
        graph.AddVertex(thingy1, playerPrefab);
        graph.AddVertex(thingy2, playerPrefab);
        graph.AddVertex(thingy3, playerPrefab);

        // Add the edges now 
        graph.AddEdge(new Edge<thingy>(thingy1, thingy2), 1);
        graph.AddEdge(new Edge<thingy>(thingy2, thingy3), 1);
        graph.AddEdge(new Edge<thingy>(thingy3, thingy1), 1);
    }

    // Update is called once per frame
    void Update()
    {
        while(counter < number)
        {
            graph.AddVertex(thingys[counter], playerPrefab);
            counter++;
        }
        
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