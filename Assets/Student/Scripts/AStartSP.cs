using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStartSP
{
    private static List<Vector2Int> AStartAlgoritm(MapGraph graph, int s, int g)
    {
        List<Vector2Int> shortestPath = new List<Vector2Int>();

        PriorityQueueMin<double,int> openNodes = new PriorityQueueMin<double,int>();
        List<int> closedNodes = new List<int>();

        //DistanceModel model = new DistanceModel(g);

        openNodes.AddKey(0,s);

        

        while (openNodes.Size() != 0)
        {
        }
        


        return shortestPath;
    }

}
