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
        
        //if(_currentPath[0].Position.x < 34 && _currentPath[0].Position.y == 35)
        //    Debug.Log($"{_currentPath.Count}, {_currentPath[_currentPath.Count - 1].GCost}");

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
        GameObject mapObstacles = GameObject.Find("Map@Obstacles");
        foreach (Transform child in mapObstacles.transform)
        {
            int centerX = (int)child.transform.position.x;
            int centerZ = (int)child.transform.position.z;
            int lenghtX = (int)child.transform.localScale.x / 2;
            int lenghtZ = (int)child.transform.localScale.z / 2;

            for (int offsetX = -lenghtX - 3; offsetX <= lenghtX + 3; offsetX++)
            {
                for (int offsetZ = -lenghtZ - 3; offsetZ <= lenghtZ + 3; offsetZ++)
                {
                    int x = centerX + offsetX;
                    int z = centerZ + offsetZ;

                    // 장애물 주변
                    if (x < centerX - lenghtX || x > centerX + lenghtX || z < centerZ - lenghtZ || z > centerZ + lenghtZ)
                    {
                        Grid[x, z].ZoneWeight = 10f;
                    }
                    // 장애물
                    else 
                    {
                        Grid[x, z].IsWalkable = false;
                    }      
                }
            }


            //Grid[(int)child.transform.position.x, (int)child.transform.position.z].ZoneWeight = 5;

            foreach (Vector2Int dir in directions)
            {
                int checkX = (int)child.transform.position.x + dir.x;
                int checkY = (int)child.transform.position.z + dir.y;

                if (checkX >= 0 && checkX < Grid.GetLength(0) && checkY >= 0 && checkY < Grid.GetLength(1) && Grid[checkX, checkY].IsWalkable)
                {
                    //Debug.Log($"{checkX}, {checkY}");
                    Grid[checkX, checkY].ZoneWeight = 50;
                }
            }
        }

        GameObject mapRemovableObstacles = GameObject.Find("Map@RemovableObstacles");
        foreach (Transform child in mapRemovableObstacles.transform)
        {
            //Grid[(int)child.transform.position.x, (int)child.transform.position.z].IsWalkable = false;
            Grid[(int)child.transform.position.x, (int)child.transform.position.z].ZoneWeight = 10.123f;
        }
    }
}
