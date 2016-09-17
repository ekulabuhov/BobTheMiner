using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// EK: translated copy of http://buildnewgames.com/astar/
public class AStar
{
    const int worldHeight = 8;
    public const int worldWidth = 8;
    int[,] world = new int[worldHeight,worldWidth];
	Point[] forrest = new Point[0];
	int maxG = int.MaxValue;

    // Path function, executes AStar algorithm operations
	public List<Point> calculatePath(Point pathStart, Point pathEnd)
    {
        Func<Point, Node, int> distanceFunction = ManhattanDistance;
        const int worldSize = worldHeight * worldWidth;
        // create Nodes from the Start and End x,y coordinates
        var mypathStart = new Node(null, pathStart);
        var mypathEnd = new Node(null, pathEnd);
        // create an array that will contain all world cells
        var AStar = new bool[worldSize];
        // list of currently open Nodes
        var Open = new List<Node>() { mypathStart };
        // list of closed Nodes
        var Closed = new List<Node>();
        // list of the final output array
        var result = new List<Point>();
        // reference to a list of Points that are nearby
        var myNeighbours = new List<Point>();
        // reference to a Node (that we are considering now)
        Node myNode;
        // reference to a Node (that starts a path in question)
        Node myPath;
        // temp integer variables used in the calculations
        int length, max, min, i, j;
        // iterate through the open list until none are left
        while (Open.Count > 0)
        {
            length = Open.Count;
            max = worldSize;
            min = -1;
            for (i = 0; i < length; i++)
            {
                if (Open[i].f < max)
                {
                    max = Open[i].f;
                    min = i;
                }
            }
            // grab the next node and remove it from Open array
            myNode = Open[min];
            Open.RemoveAt(min);
            // is it the destination node?
            if (myNode.value == mypathEnd.value)
            {
				// we can't see that far, return empty list
				if (myNode.g > maxG) {
					return new List<Point> ();
				}

                Closed.Add(myNode);
                myPath = Closed[Closed.Count - 1];
                do
                {
                    result.Add(new Point() { x = myPath.x, y = myPath.y });
                }
                while ((myPath = myPath.Parent) != null);
                // clear the working arrays
                //AStar = Closed = Open = [];
                // we want to return start to finish
                result.Reverse();
            }
            else // not the destination
            {
                // find which nearby nodes are walkable
                myNeighbours = Neighbours(myNode.x, myNode.y);
                // test each one that hasn't been tried already
                for (i = 0, j = myNeighbours.Count; i < j; i++)
                {
                    myPath = new Node(myNode, myNeighbours[i]);
                    if (!AStar[myPath.value])
                    {
                        // estimated cost of this particular route so far
						myPath.g = myNode.g + distanceFunction(myNeighbours[i], myNode) + (forrest.Contains(myNeighbours[i]) ? 15 : 0);
                        // estimated cost of entire guessed route to the destination
                        myPath.f = myPath.g + distanceFunction(myNeighbours[i], mypathEnd);
                        // remember this new path for testing above
                        Open.Add(myPath);
                        // mark this node in the world graph as visited
                        AStar[myPath.value] = true;
                    }
                }
                // remember this route as having no more untested options
                Closed.Add(myNode);
            }
        } // keep iterating until the Open list is empty
        return result;
    }

    // Returns every available North, South, East or West
    // cell that is empty. No diagonals,
    // unless distanceFunction function is not Manhattan
    private List<Point> Neighbours(int x, int y)
    {
        int N = y - 1,
          S = y + 1,
          E = x + 1,
          W = x - 1;
        bool myN = N > -1 && canWalkHere(x, N),
          myS = S < worldHeight && canWalkHere(x, S),
          myE = E < worldWidth && canWalkHere(E, y),
          myW = W > -1 && canWalkHere(W, y);
        var result = new List<Point>();

        if (myN)
            result.Add(new Point { x = x, y = N });
        if (myE)
            result.Add(new Point { x = E, y = y });
        if (myS)
            result.Add(new Point { x = x, y = S });
        if (myW)
            result.Add(new Point { x = W, y = y });
        return result;
    }

    public int ManhattanDistance(Point point, Node goal)
    {   // linear movement - no diagonals - just cardinal directions (NSEW)
        return Math.Abs(point.x - goal.x) + Math.Abs(point.y - goal.y);
    }

    /// <summary>
    /// the world data are integers:
    /// anything higher than this number is considered blocked
    /// this is handy is you use numbered sprites, more than one
    /// of which is walkable road, grass, mud, etc
    /// </summary>
    const int maxWalkableTileNum = 0;

    // returns boolean value (world cell is available and open)
    private bool canWalkHere(int x, int y)
    {
        return (world[x, y] <= maxWalkableTileNum);
    }
}
