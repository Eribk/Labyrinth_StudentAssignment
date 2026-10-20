using NUnit.Framework;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

using Unity.VisualScripting;

public class MapGraph
{
    public MapData mapData;
    public int[] vertices;
    private List<int>[] adj;
    private int e;

    public MapGraph(MapData mapData)
    {
        this.mapData = mapData;
        vertices = new int[mapData.width*mapData.height];
        for(int i=0; i<vertices.Length; i++)
        {
            adj[i] = new List<int>();
        }
        addVertices(mapData);
        addEdges(mapData);
        e = 0;
    }

    public int V()
    {
        return vertices.Length;
    }

    public int E()
    {
        return e;
    }

    public List<int> getAdj(int v)
    {
        return adj[v];
    }

    private void addEdge(int v1, int v2)
    {
        adj[v1].Add(v2);
        adj[v2].Add(v1);
        e++;

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
                addEdge(v1, v2);

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
                addEdge(v1, v2);
            }

        }
    }

}




