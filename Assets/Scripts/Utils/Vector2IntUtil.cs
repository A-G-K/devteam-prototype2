using System;
using System.Collections.Generic;
using UnityEngine;


public static class Vector2IntUtils
{
    public static int ManhattanDistance(Vector2Int from, Vector2Int to)
    {
        return Math.Abs(from.x - to.x) + Math.Abs(from.y - to.y);
    }
}

namespace Vector2IntExtension
{
    public static class Vector2IntExtensions
    {
        public static IEnumerable<Vector2Int> GetNeighbouring(this Vector2Int vector)
        {
            yield return new Vector2Int(vector.x, vector.y + 1);
            yield return new Vector2Int(vector.x + 1, vector.y);
            yield return new Vector2Int(vector.x, vector.y - 1);
            yield return new Vector2Int(vector.x - 1, vector.y);
        }
    }
}