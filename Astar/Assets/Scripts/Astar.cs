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

    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    { 
        List<Node> openNodes = new();
        HashSet<Node> closedNodes = new();
        
        var startNode = new Node(startPos, null, 0, 0);
        openNodes.Add(startNode);
        
        while (openNodes.Count != 0)
        {
            var currentNode = openNodes.OrderBy(node => node.FScore).First();
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            if (currentNode.position == endPos)
            {
                var path = new List<Vector2Int>();
                var currentPathNode = currentNode;
                
                while (currentPathNode != startNode)
                {
                    path.Add(currentPathNode.position);
                    currentPathNode = currentPathNode.parent;
                }
                
                path.Reverse();
                return path;
            }

            var gridDimensions = new Vector2Int(grid.GetLength(0), grid.GetLength(1));
            var neighbours = GetNeighbours(currentNode, gridDimensions);
            var neighboursChecked = 0;
            foreach (var neighbour in neighbours)
            {
                var currentCell = grid[currentNode.position.x, currentNode.position.y];
                var neighbourCell = grid[neighbour.position.x, neighbour.position.y];
                if (closedNodes.Any(node => node.position == neighbour.position) || IsSeparatedByWall(currentCell, neighbourCell))
                {
                    neighboursChecked++;
                    continue;
                }

                if (neighboursChecked >= neighbours.Count)
                {
                    return null;
                }

                var costToNeighbour = (int)currentNode.GScore + GetDistance(currentNode.position, neighbour.position);
                if (costToNeighbour < neighbour.GScore || openNodes.All(node => node.position != neighbour.position))
                {
                    neighbour.GScore = costToNeighbour;
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

    private bool IsSeparatedByWall(Cell currentCell, Cell neighbour)
    {
        if (currentCell.gridPosition.x > neighbour.gridPosition.x && currentCell.HasWall(Wall.LEFT))
        {
            return true;
        } if (currentCell.gridPosition.x < neighbour.gridPosition.x && currentCell.HasWall(Wall.RIGHT))
        {
            return true;
        } if (currentCell.gridPosition.y > neighbour.gridPosition.y && currentCell.HasWall(Wall.DOWN))
        {
            return true;
        } if (currentCell.gridPosition.y < neighbour.gridPosition.y && currentCell.HasWall(Wall.UP))
        {
            return true;
        }

        return false;
    }

    private List<Node> GetNeighbours(Node node, Vector2Int mazeSize)
    {
        var results = new List<Node>();
        
        for (var x = -1; x < 2; x++)
        {
            for (var y = -1; y < 2; y++)
            {
                var nodeX = node.position.x + x;
                var nodeY = node.position.y + y;
                
                if (nodeX < 0 || nodeX >= mazeSize.x || nodeY < 0 || nodeY >= mazeSize.y || Mathf.Abs(x) == Mathf.Abs(y))
                {
                    continue;
                }
                var newNode = new Node(new Vector2Int(nodeX, nodeY), null, 0, 0);
                results.Add(newNode);
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
