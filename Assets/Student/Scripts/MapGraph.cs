using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public class MapGraph
{
    public MapDataAdapter mapData;
    public int[] verticies;
    public Edge[] edges;

    public MapGraph(MapDataAdapter mapData)
    {
        this.mapData = mapData;
        verticies = new int[mapData.Width*mapData.Height];
        for(int i=0;i<mapData.Height;i++)
        {
            for(int j=0;j<mapData.Width;j++)
            {
                verticies[i*mapData.Width+j] = i * mapData.Width + j;
            }
        }
    }

    int V()
    {
        return verticies.Length;
    }

    int E()
    {
        return edges.Length;
    }

}

public class Vertix
{
    public Vector2Int position;
    public Vertix(Vector2Int position)
    {
        this.position = position;
    }

}

public class Edge
{
    public Vertex from;
    public Vertex to;
}
