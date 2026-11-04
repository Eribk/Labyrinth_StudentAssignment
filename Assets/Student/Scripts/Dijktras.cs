using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Dijktras
{
    private DirectedEdge[] edges;
    private double[] distTo;
    private PriorityQueueMin<double> queueMin;
    int start;
    MapGraph graph;


    //Constructor for Dijktras class taht finds the shortest path to all vertices to a given start vertex.
    public Dijktras(MapGraph G, int s)
    {
        start = s;
        graph = G;
        edges = new DirectedEdge[G.V()];
        distTo = new double[G.V()];
        queueMin = new PriorityQueueMin<double>(G.V());



        for (int v = 0; v < G.V(); v++)
        {
            distTo[v] = double.PositiveInfinity;
        }

        distTo[s] = 0;


        queueMin.AddKey(distTo[s], s);
        int nodeCheckedCount = 0;

        //While the priority queue is not empty, take tha vertex with the shortest path to it and relax the edges going out from the vertex. 
        while (!queueMin.isEmpty())
        {
            nodeCheckedCount++;
            int v = queueMin.delMin();
            bool isStart = (v == s);
            foreach (DirectedEdge edge in G.getAdj(v))
            {
                Relax(edge);

            }
        }

    }

    //Relaxes the edge. 
    private void Relax(DirectedEdge e)
    {
        int v = e.v;
        int w = e.edgeTo;
        double oldDist = distTo[w];

        //Checks id the current distance to a vertex is longer than the newly calculated one. 

        if (distTo[w] > distTo[v] + e.weight)
        {
            //If it is, it assigns the new distance as the shortest distance, and adds the vertex to the priority queue, or decreases its value if it is already in the queue.
            distTo[w] = distTo[v] + e.weight;
            edges[w] = e;

            if (queueMin.Visited(w))
            {
                queueMin.DecreaseKey(oldDist, distTo[w], w);
            }
            else
            {
                queueMin.AddKey(distTo[w], w);
            }
        }
    }

    //Returns the shortest path to a speciffic vertex in form of a list containing Vector2Int. 
    public List<Vector2Int> getShortestPathTo(int w)
    {

        Stack<Vector2Int> pathStack = new Stack<Vector2Int>();
        List<Vector2Int> path = new List<Vector2Int>();
        int nrOfNodes = 0;

        for (DirectedEdge e = edges[w]; e != null; e = edges[e.v])
        {
            Vector2Int pos = graph.ConvertNodeToPos(e.v);


            nrOfNodes++;
            pathStack.Push(pos);
        }

        for (int i = 0; i < nrOfNodes; i++)
        {
            Vector2Int pos = pathStack.Pop();
            path.Add(pos);
        }

        path.Add(graph.ConvertNodeToPos(w));
        return path;

    }

    //Checks if there is a path to a speciffic vetrex. 
    public bool hasPathTo(int w)
    {
        return distTo[w] < double.PositiveInfinity;

    }

}

