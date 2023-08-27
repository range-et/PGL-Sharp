using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuikGraph;

public class Simple_graph_tester : MonoBehaviour
{

    [SerializeField] public GameObject playerPrefab;

    private int number = 3;
    private int counter = 0;
    List<thingy> thingys = new List<thingy> {new thingy("D", "Vertex D"),
        new thingy("E", "Vertex E"),
        new thingy("F", "Vertex F") };
    PGL.GraphDrawer<thingy> graph;
    private Edge<thingy> edgeToDelete;
    

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
        Edge<thingy> edge1 = new Edge<thingy>(thingy1, thingy2);
        Edge<thingy> edge2 = new Edge<thingy>(thingy2, thingy3);
        Edge<thingy> edge3 = new Edge<thingy>(thingy3, thingy1);
        edgeToDelete = edge3;

        graph.AddEdge(edge1, 1, 0.1f, new Color(255,0,0));
        graph.AddEdge(edge2, 1, 0.1f, new Color(255,0,0));
        graph.AddEdge(edge3, 1, 0.1f, new Color(255,0,0));

        StartCoroutine(ExecuteAfterDelay(4.0f));
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

    private IEnumerator ExecuteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        graph.DeleteVertex(thingys[2]);
        graph.DeleteEdge(edgeToDelete);
        Debug.Log("Deleted the thing!");
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