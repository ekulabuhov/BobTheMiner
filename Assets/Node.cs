using UnityEngine;
using System.Collections;

public class Node
{
    /// <summary>
    /// The heuristic estimated cost of an entire path using this node
    /// </summary>
    public int f;
    /// <summary>
    /// The distanceFunction cost to get from the starting point to this node
    /// </summary>
    public int g;

    /// <summary>
    /// Poiner to another Node object
    /// </summary>
    public Node Parent;

    /// <summary>
    /// The location coordinates of this Node
    /// </summary>
    public int x;
    public int y;

    public int value
    {
        get
        {
            // This one is a bit of a hack
            return x + y * AStar.worldWidth;
        }
    }

    public Node(Node parent, Point point)
    {
        this.Parent = parent;
        this.x = point.x;
        this.y = point.y;
        this.f = 0;
        this.g = 0;
    }
}

public struct Point
{
    public int x;
    public int y;
}