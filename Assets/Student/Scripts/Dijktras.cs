using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Dijktras
{
    private DirectedEdge[] edges;
    private double[] distTo;
    private PriorityQueueMin<double, int> queueMin;
    int start;
    MapGraph graph;

    public Dijktras(MapGraph G, int s)
    {
        start = s;
        graph = G;
        edges = new DirectedEdge[G.V()];
        distTo = new double[G.V()];
        queueMin = new PriorityQueueMin<double, int>();

        Debug.Log("Nr of edges: " + G.E());

        int relaxed = 0;


        for (int v = 0; v < G.V(); v++)
        {
            distTo[v] = double.PositiveInfinity;
        }

        distTo[s] = 0;
        //Debug.Log("DistTo s: "+distTo[s]);
        //Debug.Log("Start node: " + s);

        queueMin.AddKey(distTo[s], s);
        int nodeCheckedCount = 0;
        while (!queueMin.isEmpty())
        {
            nodeCheckedCount++;
            //Debug.Log("Get form queue");
            int v = queueMin.delMin();
            bool isStart = (v == s);
            Debug.Log("Node: " + v +"nodeCheckCound: "+ nodeCheckedCount);
            foreach (DirectedEdge edge in G.getAdj(v))
            {
                Debug.Log("Edge to: " + edge.edgeTo + "nodeCheckCound: " + nodeCheckedCount);   
                Relax(edge);
                relaxed++;
                Debug.Log("Edges relaxed: " + relaxed);

            }
        }

    }

    private void Relax(DirectedEdge e)
    {
        int v = e.v;
        int w = e.edgeTo;
        double oldDist = distTo[w];
        Debug.Log("Edge to "+w+" weight: " + e.weight);
        Debug.Log("DistTo "+w+ " : "+ distTo[w]);
        Debug.Log("DistTo v" +v+" : " + distTo[v]);
        Debug.Log("Is dist for "+w+" shorter?: "+ (distTo[w] > distTo[v] + e.weight));
        if (distTo[w] > distTo[v] + e.weight)
        {
            Debug.Log("New distance for "+w+" is shorter");
            distTo[w] = distTo[v] + e.weight;
            edges[w] = e;

            Debug.Log("Does the queue contain "+w+"? " + queueMin.Contains(w));
            if (queueMin.Contains(w))
            {
                Debug.Log("Decrease "+w+" key");
                queueMin.DecreaseKey(oldDist, distTo[w], w);
            }
            else
            {
                Debug.Log("Add "+w+" to queue");
                queueMin.AddKey(distTo[w], w);
            }
        }
    }

    public List<Vector2Int> getShortestPathTo(int w)
    {
        Debug.Log("Has path to "+w +""+ hasPathTo(w));
        Stack<Vector2Int> pathStack = new Stack<Vector2Int>();
        List<Vector2Int> path = new List<Vector2Int>();
        int nrOfNodes = 0;
        //Debug.Log("edge w: "+edges[w].edgeTo);
        
        for (DirectedEdge e = edges[w]; e != null; e = edges[e.v])
        {
            Vector2Int pos = graph.ConvertNodeToPos(e.v);

            Debug.Log("Edge to: " + e.edgeTo + " v: " + e.v + " pos: " + pos);

            nrOfNodes++;
            pathStack.Push(pos);
        }

        //path.Add(graph.ConvertNodeToPos(start));
        for (int i = 0; i < nrOfNodes; i++)
        {
            Vector2Int pos = pathStack.Pop();
            Debug.Log("Path pos: " + pos);
            path.Add(pos);
        }

        path.Add(graph.ConvertNodeToPos(w));
        return path;
    }

    public bool hasPathTo(int w)
    {
        return distTo[w] < double.PositiveInfinity;

    }

}

