using System;
using System.Collections.Generic;
using UnityEngine;

public static class PathfindingAlgorithm
{
    // TODO: Implement the pathfinding assignment here.
    // Find the lowest-cost path from start to goal using the map dimensions, wall costs and directed jumps exposed by IMapData.
    public static List<Vector2Int> FindShortestPath(Vector2Int start, Vector2Int goal, IMapData mapData)
    {

        if(mapData == null)
        {
            Debug.Log("mapdata is null");
        }


        //Creates a graph object. 
        MapGraph graph = new MapGraph(mapData);

        //Translates start and end positions to nodes in the graph. 
        int startNode = graph.ConvertPosToNode(start);
        int endNode = graph.ConvertPosToNode(goal);


        //Assign a graph and start node to a Dijkstras object. 
        Dijktras D = new Dijktras(graph,startNode);


        //Gets shortest path from D. 
        List<Vector2Int> path = D.getShortestPathTo(endNode);

        return path; 
    }


    public static bool IsMovementBlocked(Vector2Int from, Vector2Int to, IMapData mapData)
    {
        if (mapData.TryGetJumpCost(from, to, out _))
            return false;

        int deltaX = to.x - from.x;
        int deltaY = to.y - from.y;

        if (Mathf.Abs(deltaX) + Mathf.Abs(deltaY) != 1)
            return true;

        if (deltaX != 0)
        {
            int wallX = deltaX > 0 ? to.x : from.x;
            float wallCost = mapData.GetVerticalWallCost(wallX, from.y);
            return float.IsPositiveInfinity(wallCost) || wallCost >= float.MaxValue;
        }

        int wallY = deltaY > 0 ? to.y : from.y;
        float horizontalWallCost = mapData.GetHorizontalWallCost(from.x, wallY);
        return float.IsPositiveInfinity(horizontalWallCost) || horizontalWallCost >= float.MaxValue;
    }
}
