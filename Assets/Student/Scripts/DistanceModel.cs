using Unity.VisualScripting;
using UnityEngine;

public class DistanceModel
{
    private Vector2Int goalPosition;
    public DistanceModel(Vector2Int goalNode)
    {
        goalPosition = goalNode;

    }

    public double distanceEstimate(Vector2Int v)
    {
        double distance = Mathf.Abs(goalPosition.x - v.x) + Mathf.Abs(goalPosition.y - v.y);

        return distance;


    }
    
}
