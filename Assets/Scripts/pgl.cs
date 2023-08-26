using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuikGraph;

public class PGL
{
    private static string VersionNumber = "1.0.0";

    /// <summary>
    /// This function returns the version of pgl
    /// </summary>
    /// <returns>String : The version of pgl</returns>
    public static string Version()
    {
        return VersionNumber;
    }

    // This is a wrapper class that wraps up a quik graph graph and then 
    // Stores like the position of the vertices, edges etc etc on there
    /// <summary>
    /// This is the unity wrapper around the PGL#
    /// </summary>
    public class GraphDrawer<T>
    {
        // graph drawing objects - this essetially stores the line and position maps associated with the graph
        private Dictionary<int, Vector3> positionMap = new Dictionary<int, Vector3>();
        private Dictionary<int, Drawing.Line> lineMap = new Dictionary<int, Drawing.Line>();
        // a dictionary that contains the original mapping of the vertices and the corresponding index that is 
        private Dictionary<int, T> vertexMap = new Dictionary<int, T>();
        // being registered and stored on the index map
        private AdjacencyGraph<T, Edge<T>> graph = new AdjacencyGraph<T, Edge<T>>();
        public float bounds = 10; // 10 is the defaut value
        private GameObject masterGameObject;

        public GraphDrawer(float bounds)
        {
            // set the current bounds
            this.bounds = bounds;
            // create like a master gameObject so that all things like lines
            // and vertices are all added to that gameobject
            masterGameObject = CreateUniqueGraphDrawingObject();
            // Debug log 
            Debug.Log(masterGameObject.name);
        }

        /// <summary>
        /// An initializing function, this declares a gameobject inside which are 
        /// A gameobject containing the vertices 
        /// A gameobject containing the edges
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        private GameObject CreateUniqueGraphDrawingObject(string objectName = null)
        {
            string uniqueID = Guid.NewGuid().ToString();

            GameObject newObject = new GameObject(objectName ?? "Object_" + uniqueID);

            GameObject verticesObject = new GameObject("Verties");
            GameObject edgesObject = new GameObject("Edges");

            verticesObject.transform.SetParent(newObject.transform);
            edgesObject.transform.SetParent(newObject.transform);

            return newObject;
        }

        ///////////////////////////////////
        // General Graph methods like adding an edge / Deleteing an edge etc
        ///////////////////////////////////

        static string DictionaryToString<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            List<string> keyValuePairs = new List<string>();
            foreach (var kvp in dictionary)
            {
                keyValuePairs.Add($"{kvp.Key}: {kvp.Value}");
            }

            return string.Join(", ", keyValuePairs);
        }

        public void GetMaps()
        {
            Debug.Log(DictionaryToString(positionMap));
            Debug.Log(DictionaryToString(lineMap));
            Debug.Log(DictionaryToString(vertexMap));
        }

        /// <summary>
        /// Find the corresponding vertex in a graph
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="success"></param>
        /// <param name="foundVertex"></param>
        public void TryGetvertexFromGraph(T vertex, out bool success, out T foundVertex)
        {
            success = false;
            foundVertex = default;

            foreach (T vert in graph.Vertices)
            {
                if (vert.Equals(vertex))
                {
                    foundVertex = vert;
                    success = true;
                    break;
                }
            }
        }

        public void GetMapIndex(T vertex, out bool success, out int VertexIndex)
        {
            VertexIndex = -1;
            success = false;

            foreach(KeyValuePair<int, T> kvp in vertexMap)
            {
                if(kvp.Value.Equals(vertex)) 
                {
                    VertexIndex = kvp.Key;
                    success = true;
                    break;
                }
            }
        }

        /// <summary>
        /// A method to add an vertex
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public bool AddVertex(T vertex)
        {
            // try adding a vertex to the graph
            bool result = graph.AddVertex(vertex);
            // this returns true if the operation was sucessful
            if (result)
            {
                positionMap[graph.VertexCount] = Drawing.randomPosition(bounds);
                vertexMap[graph.VertexCount] = vertex;
                // then add the mesh to the screen (remember that the mesh kinda lerps in)
            }
            return result;
        }

        /// <summary>
        /// A method to add an edge
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="divisions"></param>
        /// <returns></returns>
        public bool AddEdge(Edge<T> edge, float distance)
        {
            bool result = graph.AddEdge(edge);
            if (result)
            {
                int startIndex;
                bool startFound;
                GetMapIndex(edge.Source, out startFound, out startIndex);

                int endIndex;
                bool endFound;
                GetMapIndex(edge.Source, out endFound, out endIndex);

                // First get the relevant start and end points
                Drawing.Line line = new Drawing.Line();
                line.ConstructLineDistance(positionMap[startIndex], positionMap[endIndex], distance);
                lineMap[graph.EdgeCount] = line;

            }
            return result;
        }
    }

    // This is the main drawing class 
    // This has the stuff like the notion of a line 
    // (Points are directly borrowed from the Vector3 representation in Unity)
    public class Drawing
    {
        // And there is a line class that stores a list of points in it
        // Again Points are vectors in this method
        public class Line
        {
            public List<Vector3> points = new List<Vector3>();

            /// <summary>
            /// This creates the line thing, the reason for this is so that once a 
            /// Line is made there is no need to delete it, we can keep it and then keep modifying 
            /// the relevant parameters
            /// </summary>
            /// <param name="start"></param>
            /// <param name="end"></param>
            /// <param name="divisions"></param>
            public void ConstructLineDivisions(Vector3 start, Vector3 end, int divisions)
            {
                points.Clear();
                float lerpfraction;
                for (int i = 0; i < divisions; i++)
                {
                    // calculate the fraction between the two points 
                    lerpfraction = (float)i / divisions;
                    Vector3 point = Vector3.Lerp(start, end, lerpfraction);
                    points.Add(point);
                }
            }

            /// <summary>
            /// Draw an edge and split the line into approximately the same distance 
            /// The distances are always a little less if not exact
            /// </summary>
            /// <param name="start"></param>
            /// <param name="end"></param>
            /// <param name="distance"></param>
            public void ConstructLineDistance(Vector3 start, Vector3 end, float distance)
            {
                int numberOfDivisions = (int)Math.Ceiling(Vector3.Distance(start, end)/distance);
                ConstructLineDivisions(start, end, numberOfDivisions);
            }
        }

        public static Vector3 randomPosition(float bound)
        {
            System.Random random = new System.Random();
            float x = ((float)random.NextDouble()) * bound;
            float y = ((float)random.NextDouble()) * bound;
            float z = ((float)random.NextDouble()) * bound;
            Vector3 position = new Vector3(x, y, z);
            return position;
        }

        public static Dictionary<int, Vector3> RandomizePointPositions(int numberOfVertices, float bound)
        {
            Dictionary<int, Vector3> positonMap = new Dictionary<int, Vector3>();
            for (int i = 0; i < numberOfVertices; i++)
            {
                positonMap[i] = randomPosition(bound);
            }
            return positonMap;
        }

        public static Dictionary<int, Vector3> GenerateSphericalPoints(int numPts)
        {
            Dictionary<int, Vector3> positionMap = new Dictionary<int, Vector3>();
            double[] indices = new double[numPts];

            for (int i = 0; i < numPts; i++)
            {
                indices[i] = i + 0.5;
            }

            double phi, theta;
            double[] x = new double[numPts];
            double[] y = new double[numPts];
            double[] z = new double[numPts];

            for (int i = 0; i < numPts; i++)
            {
                phi = Math.Acos(1 - 2 * indices[i] / numPts);
                theta = Math.PI * (1 + Math.Sqrt(5)) * indices[i];

                x[i] = Math.Cos(theta) * Math.Sin(phi);
                y[i] = Math.Sin(theta) * Math.Sin(phi);
                z[i] = Math.Cos(phi);

                Vector3 point = new Vector3((float)x[i], (float)y[i], (float)z[i]);
                positionMap[i] = point;
            }

            return positionMap;
        }
    }

    public class Simulation
    {
        // Fruchterman-Reingold (Kamada Kawai modified)
        class FruchtermanReingold
        {
            // set a position map 

            // caluclate an iteration of that position map
        }

        // Edge bundling
        class KDEEdgeBundling
        {
            // a private simulation matrix / 3d grod that stores all the points
            
            // calculate an iteration of that
        }
    }
}