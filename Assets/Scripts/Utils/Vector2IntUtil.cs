using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector2IntExtension;


public static class Vector2IntUtils
{
    public static int ManhattanDistance(Vector2Int from, Vector2Int to)
    {
        return Math.Abs(from.x - to.x) + Math.Abs(from.y - to.y);
    }

    public static IEnumerable<Vector2Int> GetNearbyCells(Vector2Int selectedCell, int limit)
    {
        var result = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(selectedCell);

        while (queue.Count != 0)
        {
            Vector2Int current = queue.Dequeue();
            
            foreach (Vector2Int neighbourCell in current.GetNeighbouring())
            {
                if (ManhattanDistance(selectedCell, neighbourCell) <= limit)
                {
                    result.Add(neighbourCell);
                    queue.Enqueue(neighbourCell);
                }
            }
        }

        return result;
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