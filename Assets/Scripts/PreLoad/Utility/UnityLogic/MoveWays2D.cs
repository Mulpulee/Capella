using System;
using UnityEngine;

public enum MoveWayType2D
{
    Up, Down, Left, Right, Stop
}
public static class MoveWays2D
{
    public static Vector2 MovewayToVector(MoveWayType2D moveWay)
    {
        switch (moveWay)
        {
            case MoveWayType2D.Up:
                return new Vector2(0, 1);
            case MoveWayType2D.Left:
                return new Vector2(-1, 0);
            case MoveWayType2D.Right:
                return new Vector2(1, 0);
            case MoveWayType2D.Down:
                return new Vector2(0, -1);
            case MoveWayType2D.Stop:
                return Vector2.zero;
        }
        Debug.LogError($"{moveWay} is not implemented");
        return Vector2.zero;
    }

    public static Vector3 MovewayToVectorDot5D(MoveWayType2D moveWay)
    {
        switch (moveWay)
        {
            case MoveWayType2D.Up:
                return new Vector3(0, 0, 1);
            case MoveWayType2D.Left:
                return new Vector3(-1, 0, 0);
            case MoveWayType2D.Right:
                return new Vector3(1, 0, 0);
            case MoveWayType2D.Down:
                return new Vector3(0, 0, -1);
            case MoveWayType2D.Stop:
                return Vector3.zero;
        }
        Debug.LogError($"{moveWay} is not implemented");
        return Vector3.zero;
    }
}