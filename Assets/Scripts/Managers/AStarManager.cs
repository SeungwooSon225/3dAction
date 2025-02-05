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

    List<Node> _barrelList;

    public float CurrentCost { get; set; }

    Vector2Int[] directions = {
            new Vector2Int(-1, 0), new Vector2Int(1, 0),    // 좌우
            new Vector2Int(0, -1), new Vector2Int(0, 1),    // 상하
            new Vector2Int(-1, -1), new Vector2Int(1, 1),   // 대각선
            new Vector2Int(1, -1), new Vector2Int(-1, 1),   // 대각선
        };


    public void Init()
    {
        _barrelList = new List<Node>();

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

        CurrentCost = _currentPath.Count;

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

            for (int offsetX = -lenghtX - 1; offsetX <= lenghtX + 1; offsetX++)
            {
                for (int offsetZ = -lenghtZ - 1; offsetZ <= lenghtZ + 1; offsetZ++)
                {
                    int x = centerX + offsetX;
                    int z = centerZ + offsetZ;

                    // 장애물 주변
                    if (x < centerX - lenghtX || x > centerX + lenghtX || z < centerZ - lenghtZ || z > centerZ + lenghtZ)
                    {
                        Grid[x, z].ZoneWeight = 5f;
                    }
                    // 장애물
                    else 
                    {
                        Grid[x, z].NodeType = NodeType.Obstacle;
                        Grid[x, z].IsWalkable = false;

                        //Managers.Resource.Instantiate("RedCube").transform.position = new Vector3(x, 0f, z);

                    }      
                }
            }
        }

        GameObject mapRemovableObstacles = GameObject.Find("Map@RemovableObstacles");
        foreach (Transform child in mapRemovableObstacles.transform)
        {
            int centerX = (int)child.transform.position.x;
            int centerZ = (int)child.transform.position.z;
            int lenghtX = (int)child.transform.localScale.x / 2;
            int lenghtZ = (int)child.transform.localScale.z / 2;

            for (int offsetX = -lenghtX; offsetX <= lenghtX; offsetX++)
            {
                for (int offsetZ = -lenghtZ; offsetZ <= lenghtZ; offsetZ++)
                {
                    int x = centerX + offsetX;
                    int z = centerZ + offsetZ;

                    Grid[x, z].NodeType = NodeType.RemovableObstacle;
                    Grid[x, z].IsWalkable = true;
                    Grid[x, z].ZoneWeight = 5f;
                    Grid[x, z].Object = child.gameObject;

                    //Managers.Resource.Instantiate("BlueCube").transform.position = new Vector3(x, 0f, z);
                }
            }

            if (child.name == "Barrel")
            {
                Grid[centerX, centerZ].NodeType = NodeType.Barrel;
                _barrelList.Add(Grid[centerX, centerZ]);
            }
        }
    }

    public void RemoveObstacle(Transform obstacle)
    {
        int centerX = (int)obstacle.position.x;
        int centerZ = (int)obstacle.position.z;
        int lenghtX = (int)obstacle.localScale.x / 2;
        int lenghtZ = (int)obstacle.localScale.z / 2;

        for (int offsetX = -lenghtX - 3; offsetX <= lenghtX + 3; offsetX++)
        {
            for (int offsetZ = -lenghtZ - 3; offsetZ <= lenghtZ + 3; offsetZ++)
            {
                int x = centerX + offsetX;
                int z = centerZ + offsetZ;

                // 장애물 주변
                if (x < centerX - lenghtX || x > centerX + lenghtX || z < centerZ - lenghtZ || z > centerZ + lenghtZ)
                {
                    if (Grid[x, z].ZoneWeight < 1.9f)
                        Grid[x, z].ZoneWeight = 0f;
                }
                // 장애물
                else
                {
                    Grid[x, z].Object = null;
                    Grid[x, z].NodeType = NodeType.None;
                    Grid[x, z].ZoneWeight = 0f;
                    Grid[x, z].IsWalkable = true;
                }
            }
        }

        if (obstacle.name == "Barrel")
        {
            _barrelList.Remove(Grid[centerX, centerZ]);
        }
    }

    public void IncreaseWeight(Transform obstacle, int size)
    {
        int centerX = (int)obstacle.position.x;
        int centerZ = (int)obstacle.position.z;
        int lenghtX = (int)obstacle.localScale.x / 2;
        int lenghtZ = (int)obstacle.localScale.z / 2;

        for (int offsetX = -lenghtX - size; offsetX <= lenghtX + size; offsetX++)
        {
            for (int offsetZ = -lenghtZ - size; offsetZ <= lenghtZ + size; offsetZ++)
            {
                int x = centerX + offsetX;
                int z = centerZ + offsetZ;
               
                Grid[x, z].ZoneWeight += 5f;              
            }
        }
    }

    public Node FindNearBarrel(Vector3 pos, float dis)
    {
        foreach (Node n in _barrelList)
        {
            if ((new Vector3(n.Position.x, 0f, n.Position.y) - pos).magnitude < dis && n.Object.GetComponent<BarrelStat>().Hp == 1)
            {
                return n;
            }
        }

        return null;
    }
}
