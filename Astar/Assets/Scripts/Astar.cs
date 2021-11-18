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

    private List<Node> openNodes = new List<Node>();
    private List<Node> closedNodes = new List<Node>();

    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        //Add start node to the open list
        var startNode = new Node(startPos, null, 0, 0);
        openNodes.Add(startNode);

        Node currentNode;

        while (openNodes.Count > 0)
        {
            currentNode = openNodes.OrderBy(node => node.FScore).First();

            //Found Goal
            if (currentNode.position == endPos)
            {
                return null;
            }
            
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            foreach (var cell in grid)
            {
                if (openNodes.Any(node => node.position == cell.gridPosition))
                {
                    
                }
            }
        }

        return null;
    }
    
    private List<Node> GetNeighbours(Node node, Vector2Int mazeSize)
    {
        var result = new List<Node>();
        for (var x = -1; x < 1; x++)
        {
            for (var y = -1; y < 1; y++)
            {
                var nodeX = node.position.x + x;
                var nodeY = node.position.y + y;
                if (nodeX < 0 || nodeX >= mazeSize.x || nodeY < 0 || nodeY >= mazeSize.y || Mathf.Abs(x) == Mathf.Abs(y))
                {
                    continue;
                }
                //result.Add(candidateNode);
            }
        }
        return result;
    }

    private int GetDistance(Node nodeA, Node nodeB) {
        var dstX = Mathf.Abs(nodeA.position.x - nodeB.position.x);
        var dstY = Mathf.Abs(nodeA.position.y - nodeB.position.y);

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
