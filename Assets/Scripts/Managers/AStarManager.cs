using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarManager
{
    public Node[,] Grid;

    public GameObject testGo;

    public Transform StartPoint;
    public Transform EndPoint;

    public AStarPathfinding _pathfinding;
    public List<Node> _currentPath;

    Vector2Int[] directions = {
            new Vector2Int(-1, 0), new Vector2Int(1, 0),    // 좌우
            new Vector2Int(0, -1), new Vector2Int(0, 1),    // 상하
            new Vector2Int(-1, -1), new Vector2Int(1, 1),   // 대각선
            new Vector2Int(1, -1), new Vector2Int(-1, 1),   // 대각선
        };


    public void Init()
    {
        InitializeGrid(60, 60);
        _pathfinding = new AStarPathfinding(Grid);
    }


    public Node FindPath(GameObject start, GameObject target)
    {
        _pathfinding.ResetGrid(Grid);
        _currentPath = _pathfinding.FindPath(new Vector2Int((int)Mathf.Round(start.transform.position.x), (int)Mathf.Round(start.transform.position.z)),
                                             new Vector2Int((int)target.transform.position.x, (int)target.transform.position.z));

        if (_currentPath == null) return null;
        
        Debug.Log($"{_currentPath.Count}, {_currentPath[_currentPath.Count - 1].GCost}");

        return _currentPath[0];
    }

    private void InitializeGrid(int width, int height)
    {
        Grid = new Node[width + 1, height + 1];

        for (int x = 0; x <= width; x++)
        {
            for (int y = 0; y <= height; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                Grid[x, y] = new Node(position, true);
            }
        }

        // To Do
        GameObject map = GameObject.Find("Map");

        foreach (Transform child in map.transform)
        {
            if (child.name == "Obstacle")
            {
                Grid[(int)child.transform.position.x, (int)child.transform.position.z].IsWalkable = false;
                //Grid[(int)child.transform.position.x, (int)child.transform.position.z].ZoneWeight = 5;

                foreach (Vector2Int dir in directions)
                {
                    int checkX = (int)child.transform.position.x + dir.x;
                    int checkY = (int)child.transform.position.z + dir.y;

                    if (checkX >= 0 && checkX < Grid.GetLength(0) && checkY >= 0 && checkY < Grid.GetLength(1) && Grid[checkX, checkY].IsWalkable)
                    {
                        //Debug.Log($"{checkX}, {checkY}");
                        Grid[checkX, checkY].ZoneWeight += 50;
                    }
                }
            }
        }
    }
}
