using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public class MapGraph
{
    public MapData mapData;
    public int[] vertices;
    public Edge[] edges;

    public MapGraph(MapData mapData)
    {
        this.mapData = mapData;
        vertices = new int[mapData.width*mapData.height];
        edges= new Edge[(mapData.width-1)*mapData.height + (mapData.height-1)*mapData.width];
        addVertices(mapData);
        addEdges(mapData);
    }

    int V()
    {
        return vertices.Length;
    }

    int E()
    {
        return edges.Length;
    }

    private void addEdge(int v1, int v2,int index)
    {
        Edge edge = new Edge(v1, v2);
        edges[index] = edge;
    }

    private void addVertices(MapData mapData)
    {
        for (int i = 0; i < mapData.height; i++)
        {
            for (int j = 0; j < mapData.width; j++)
            {
                vertices[i * mapData.width + j] = i * mapData.width + j;
            }
        }
    }

    private void addEdges(MapData mapData)
    {
        int currentIndex = 0;

        for (int i = 0; i < vertices.Length; i++)
        {
            bool vWallExists = false;
            bool hWallExists = false;
            int currentX = i % mapData.width;
            int currentY = i / mapData.width;
            for (int j = 0; j < mapData.vwalls.Length; j++)
            {
                
                Wall wall = mapData.vwalls[j];
                if (wall.x == currentX && wall.y == currentY)
                {
                    vWallExists = true;
                    break;
                }
            }

            if (!vWallExists&&i+1<vertices.Length)
            {
                int v1 = i;
                int v2 = i + 1;
                addEdge(v1, v2, currentIndex);
                currentIndex++;
            }

            for(int j = 0; j < mapData.hwalls.Length; j++)
            {
                Wall wall = mapData.hwalls[j];
                if (wall.x == currentX && wall.y == currentY)
                {
                    hWallExists = true;
                    break;
                }
            }

            if(!hWallExists&&i+mapData.width<vertices.Length)
            {
                int v1 = i;
                int v2 = i + mapData.width;
                addEdge(v1, v2, currentIndex);
                currentIndex++;
            }

        }
    }

}



public class Edge
{
    public int vertex1;
    public int vertex2;

    public Edge(int v1, int v2)
    {
        vertex1 = v1;
        vertex2 = v2;
    }
}
