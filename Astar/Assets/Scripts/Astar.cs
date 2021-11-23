using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Astar
{
    /// <summary>
    /// TODO: Implement this function so that it returns a list of Vector2Int positions which describes a path
    /// Note that you will probably need to add some helper functions
    /// from the startPos to the endPos
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="grid"></param>
    /// <returns></returns>

    private List<Node> openNodes = new();
    private HashSet<Node> closedNodes = new();

    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        //Add start node to the open list
        var startNode = new Node(startPos, null, 0, 0);
        openNodes.Add(startNode);

        while (openNodes.Count > 0)
        {
            var currentNode = openNodes.OrderBy(node => node.FScore).First();
            Debug.Log(openNodes.Count);

            //Found Goal
            if (currentNode.position == endPos)
            {
                var path = new List<Vector2Int>();
                var currentPos = endPos;

                while (currentPos != startPos)
                {
                    path.Add(currentPos);
                    foreach (var node in closedNodes.Where(node => node.position == currentPos))
                    {
                        currentPos = node.parent.position;
                    }
                }

                path.Reverse();
                return path;
            }

            foreach (var node in openNodes)
            {
                if (!(node.FScore <= currentNode.FScore)) continue;
                if (node.HScore < currentNode.HScore)
                {
                    currentNode = node;
                }
            }
            
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            foreach (var neighbour in GetNeighbours(currentNode, new Vector2Int(grid.GetLength(0), grid.GetLength(1))))
            {
                if (closedNodes.Any(node => node.position == neighbour.position))
                {
                    continue;
                }

                var newScore = currentNode.GScore + GetDistance(currentNode.position, neighbour.position);
                if (!(newScore < neighbour.GScore) && openNodes.Any(node => node.position == neighbour.position)) continue;
                {
                    neighbour.GScore = newScore;
                    neighbour.HScore = GetDistance(neighbour.position, endPos);
                    neighbour.parent = currentNode;

                    if (openNodes.All(node => node.position != neighbour.position))
                    {
                        openNodes.Add(neighbour);
                    }
                }
            }
        }

        return null;
    }
    
    private List<Node> GetNeighbours(Node node, Vector2Int mazeSize)
    {
        var results = new List<Node>();
        
        for (var x = -1; x < 2; x++)
        {
            for (var y = -1; y < 2; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                var nodeX = node.position.x + x;
                var nodeY = node.position.y + y;
                
                if (nodeX < 0 || nodeX >= mazeSize.x || nodeY < 0 || nodeY >= mazeSize.y || Mathf.Abs(x) == Mathf.Abs(y))
                {
                    var newNode = new Node(new Vector2Int(nodeX, nodeY), null, 0, 0);
                    results.Add(newNode);
                }
            }
        }
        return results;
    }

    private int GetDistance(Vector2Int nodeA, Vector2Int nodeB) {
        var dstX = Mathf.Abs(nodeA.x - nodeB.x);
        var dstY = Mathf.Abs(nodeA.y - nodeB.y);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }

    /// <summary>
    /// This is the Node class you can use this class to store calculated FScores for the cells of the grid, you can leave this as it is
    /// </summary>
    public class Node
    {
        public Vector2Int position; //Position on the grid
        public Node parent; //Parent Node of this node

        public float FScore { //GScore + HScore
            get { return GScore + HScore; }
        }
        public float GScore; //Current Travelled Distance
        public float HScore; //Distance estimated based on Heuristic
        
        public Node() { }
        public Node(Vector2Int position, Node parent, int GScore, int HScore)
        {
            this.position = position;
            this.parent = parent;
            this.GScore = GScore;
            this.HScore = HScore;
        }
    }
}
