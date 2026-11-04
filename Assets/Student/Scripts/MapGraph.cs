using NUnit.Framework;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

using Unity.VisualScripting;

public class MapGraph
{
    public IMapData mapData;
    public int[] vertices;
    private List<DirectedEdge>[] adj;
    private int e;
    private List<DirectedEdge> edges;

    public MapGraph(IMapData mapData)
    {
        this.mapData = mapData;
        vertices = new int[mapData.Width*mapData.Height];
        edges=new List<DirectedEdge>();

        addVertices(mapData);

        adj = new List<DirectedEdge>[V()];
        for(int i=0; i<vertices.Length; i++)
        {
            adj[i] = new List<DirectedEdge>();
        }
        addEdges(mapData);
    }

    //Returns the number of vertices in the graph. 
    public int V()
    {
        return vertices.Length;
    }

    //Returns the nnumber of edges in the graph.
    public int E()
    {
        return e;
    }

    //Returns a list of edges adjacent to a specific vertex. 
    public List<DirectedEdge> getAdj(int v)
    {
        return adj[v];
    }


    //Creates a directed edge and adds them to edges.
    private void addDirectedEdge(int from, int to, float weight)
    {
        DirectedEdge edge = new DirectedEdge(from, to, weight);
        edges.Add(edge);
        adj[from].Add(edge);
        e++;
    }

    //Creates and adds all vertices to the graph. 
    private void addVertices(IMapData mapData)
    {
        for (int i = 0; i < mapData.Height; i++)
        {
            for (int j = 0; j < mapData.Width; j++)
            {
                vertices[i * mapData.Width + j] = i * mapData.Width + j;
            }
        }
    }

    //Creates all of the eddges for the graph. 
    private void addEdges(IMapData mapData)
    {

        //Goes through every vertex in the graph. 
        for (int i = 0; i < vertices.Length; i++)
        {
            //Gets the x and y positions for the current vertex. 
            int currentX = i % mapData.Width;
            int currentY = i / mapData.Width;

            //Checks if there are any jomps from that vertex, and if so, adds them to the edges- 
            if(mapData.HasJumpFrom(currentX, currentY))
            {
                List<JumpConnection> jumps = mapData.GetJumpsFrom(new Vector2Int(currentX, currentY));
                
                foreach(JumpConnection jump in jumps)
                {
                    int v1 = i;
                    int v2 = ConvertPosToNode(jump.To);
                    float weight = jump.Cost;
                    addDirectedEdge(v1, v2, weight);
                }
            }


            //Adds and gets the cost for the horisontal edges in the graph.
            if (i + 1 < vertices.Length && currentX != mapData.Width - 1)
            {
                int v1 = i;
                int v2 = i + 1;
                Vector2Int from = new Vector2Int(currentX, currentY);
                Vector2Int to = new Vector2Int(v2%mapData.Width, v2/mapData.Width);
                float weigth = GetMovementCost(from,to);
                addDirectedEdge(v1, v2, weigth);
                addDirectedEdge(v2, v1, weigth);

            }

            //Adds and gets the cost for the vertical edges in the graph.
            if (i + mapData.Width < vertices.Length)
            {
                int v1 = i;
                int v2 = i + mapData.Width;
                Vector2Int from = new Vector2Int(currentX, currentY);
                Vector2Int to = new Vector2Int(v2 % mapData.Width, v2 / mapData.Width);
                float weight = GetMovementCost(from, to);
                addDirectedEdge(v1, v2, weight);
                addDirectedEdge(v2, v1, weight);
            }

        }

    }

    // Translates a node in the graph into a Vector2Int position. 
    public Vector2Int ConvertNodeToPos(int v)
    {
        int x=v%mapData.Width;
        int y=v/mapData.Width;

        Vector2Int pos=new Vector2Int(x,y);
        return pos;
    }

    // Translates a Vector2Int position into a node in the graph. 
    public int ConvertPosToNode(Vector2Int pos)
    {
        int node = (mapData.Width * pos.y)+pos.x;
        if (node < 0 || node >= vertices.Length)
        {
            throw new System.ArgumentOutOfRangeException($"Position {pos} is out of bounds for the map dimensions.");
        }
        return node;
    }


    //Borrowed methods from the PathfinderManager class to calculate movement costs between two positions on the map.
    private float GetMovementCost(Vector2Int from, Vector2Int to)
    {
        if (mapData.TryGetJumpCost(from, to, out float jumpCost))
            return jumpCost;

        float baseCost = 1f;
        int deltaX = to.x - from.x;
        int deltaY = to.y - from.y;

        if (deltaX != 0)
        {
            int wallX = deltaX > 0 ? to.x : from.x;
            float wallCost = mapData.GetVerticalWallCost(wallX, from.y);
            if (wallCost > 1f) baseCost = wallCost;
        }
        else if (deltaY != 0)
        {
            int wallY = deltaY > 0 ? to.y : from.y;
            float wallCost = mapData.GetHorizontalWallCost(from.x, wallY);
            if (wallCost > 1f) baseCost = wallCost;
        }

        return baseCost;
    }

}



// A class repressenting a directed edge. 
public class DirectedEdge
{
    public int v;
    public int edgeTo;
    public float weight;

    public DirectedEdge(int v, int edgeTo, float weight)
    {
        this.v = v;
        this.edgeTo = edgeTo;
        this.weight = weight;
    }
}







