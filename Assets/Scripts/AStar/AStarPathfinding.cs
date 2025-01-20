using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AStarPathfinding
{
    private Node[,] _grid;
    private int _gridWidth, _gridHeight;

    public AStarPathfinding(Node[,] grid)
    {
        _grid = grid;
        _gridWidth = grid.GetLength(0);
        _gridHeight = grid.GetLength(1);
    }

    Vector2Int[] directions = {
            new Vector2Int(-1, 0), new Vector2Int(1, 0),    // 좌우
            new Vector2Int(0, -1), new Vector2Int(0, 1),    // 상하
            new Vector2Int(-1, -1), new Vector2Int(1, 1),   // 대각선
            new Vector2Int(1, -1), new Vector2Int(-1, 1),   // 대각선
        };

    public void ResetGrid(Node[,] grid)
    {
        foreach (var node in grid)
        {
            node.GCost = 99999;
            node.HCost = 0;
            node.Parent = null;
        }
    }

    public List<Node> FindPath(Vector2Int start, Vector2Int target)
    {
        //Debug.Log("Start FindPath!");

        Node startNode = _grid[start.x, start.y];
        startNode.GCost = 0;
        Node targetNode = _grid[target.x, target.y];

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);

        int count = 0;

        while (openList.Count > 0)
        {
            count++;
            Node currentNode = openList.OrderBy(n => n.Fcost).ThenBy(n => n.GCost).First();

            //Debug.Log($"{count}: {currentNode.Position.x}, {currentNode.Position.y} : {currentNode.GCost}, {currentNode.HCost}");

            if (currentNode == targetNode)
                return RetracePath(startNode, targetNode);

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (closedList.Contains(neighbor))
                    continue;                                                                                                                                                                                                                                                          

                float newCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor) + neighbor.ZoneWeight;

                if (newCostToNeighbor < neighbor.GCost || !openList.Contains(neighbor))
                {
                    neighbor.GCost = newCostToNeighbor;
                    neighbor.HCost = GetDistance(neighbor, targetNode);
                    neighbor.Parent = currentNode;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);

                    //Debug.Log($"{count}: {neighbor.Position.x}, {neighbor.Position.y} : {currentNode.GCost}, {currentNode.HCost}");
                }
            }
        }

        return null;
    }

    private float GetDistance(Node a, Node b)
    {
        //return (a.Position - b.Position).magnitude;

        int dx = Mathf.Abs(a.Position.x - b.Position.x);
        int dy = Mathf.Abs(a.Position.y - b.Position.y);
        float h = dx > dy ? 1.414f * dy + (dx - dy) : 1.414f * dx + (dy - dx);
        return h;
    }

    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        foreach (Vector2Int dir in directions)
        {
            int checkX = node.Position.x + dir.x;
            int checkY = node.Position.y + dir.y;

            if (checkX >= 0 && checkX < _gridWidth && checkY >= 0 && checkY < _gridHeight && _grid[checkX, checkY].IsWalkable)
            {
                // 대각선 이동 조건 추가
                if (dir.x != 0 && dir.y != 0)
                {
                    if (!_grid[node.Position.x + dir.x, node.Position.y].IsWalkable ||
                        !_grid[node.Position.x, node.Position.y + dir.y].IsWalkable)
                    {
                        continue;
                    }
                }

                neighbors.Add(_grid[checkX, checkY]);
            }
        }

        return neighbors;
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }
}
