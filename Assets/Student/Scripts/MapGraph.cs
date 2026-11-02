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

    public int V()
    {
        return vertices.Length;
    }

    public int E()
    {
        return e;
    }

    public List<DirectedEdge> getAdj(int v)
    {
        return adj[v];
    }

    private void addDirectedEdge(int from, int to, float weight)
    {
        DirectedEdge edge = new DirectedEdge(from, to, weight);
        edges.Add(edge);
        adj[from].Add(edge);
        e++;
    }

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

    private void addEdges(IMapData mapData)
    {

        for (int i = 0; i < vertices.Length; i++)
        {
            //bool vWallExists = false;
            //bool hWallExists = false;
            int currentX = i % mapData.Width;
            int currentY = i / mapData.Width;

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

            //for (int j = 0; j < mapData.vwalls.Length; j++)
            //{

            //    Wall wall = mapData.vwalls[j];
            //    if (wall.x == currentX && wall.y == currentY)
            //    {
            //        vWallExists = true;
            //        break;
            //    }
            //}

            if (i + 1 < vertices.Length && currentX != mapData.Width - 1)
            {
                int v1 = i;
                int v2 = i + 1;
                Vector2Int from = new Vector2Int(currentX, currentY);
                Vector2Int to = new Vector2Int(v2%mapData.Width, v2/mapData.Width);
                float weigth = GetMovementCost(from,to);
                //Debug.Log("Weight: " + weigth);
                addDirectedEdge(v1, v2, weigth);
                addDirectedEdge(v2, v1, weigth);

            }

            ////for (int j = 0; j < mapData.hwalls.Length; j++)
            ////{
            ////    Wall wall = mapData.hwalls[j];
            ////    if (wall.x == currentX && wall.y == currentY)
            ////    {
            ////        hWallExists = true;
            ////        break;
            ////    }
            ////}

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


        //    for(int x=0; x<mapData.Width; x++)
        //    {
        //        for(int y=0; y<mapData.Height; y++)
        //        {
        //            //bool vWallExists = false;
        //            //bool hWallExists = false;
        //            int currentX = x;
        //            int currentY = y;
        //            int i = y * mapData.Width + x;
        //            //for (int j = 0; j < mapData.vwalls.Length; j++)
        //            //{

        //            //    Wall wall = mapData.vwalls[j];
        //            //    if (wall.x == currentX && wall.y == currentY)
        //            //    {
        //            //        vWallExists = true;
        //            //        break;
        //            //    }
        //            //}



        //            if (i + 1 < vertices.Length && currentX != mapData.Width - 1)
        //            {
        //                int v1 = i;
        //                int v2 = i + 1;
        //                float weigth = mapData.GetVerticalWallCost(currentX, currentY);
        //                //Debug.Log("Weight: " + weigth);
        //                addDirectedEdge(v1, v2, weigth);
        //                addDirectedEdge(v2, v1, weigth);

        //            }

        //            //for (int j = 0; j < mapData.hwalls.Length; j++)
        //            //{
        //            //    Wall wall = mapData.hwalls[j];
        //            //    if (wall.x == currentX && wall.y == currentY)
        //            //    {
        //            //        hWallExists = true;
        //            //        break;
        //            //    }
        //            //}

        //            if (i + mapData.Width < vertices.Length)
        //            {
        //                int v1 = i;
        //                int v2 = i + mapData.Width;
        //                float weight = mapData.GetHorizontalWallCost(currentX, currentY);
        //                Debug.Log("Weight: " + weight);
        //                addDirectedEdge(v1, v2, weight);
        //                addDirectedEdge(v2, v1, weight);
        //            }

        //        }
        //    }
    }

    public Vector2Int ConvertNodeToPos(int v)
    {
        int x=v%mapData.Width;
        int y=v/mapData.Width;

        Vector2Int pos=new Vector2Int(x,y);
        return pos;
    }

    public int ConvertPosToNode(Vector2Int pos)
    {
        int node = (mapData.Width * pos.y)+pos.x;
        return node;
    }

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







