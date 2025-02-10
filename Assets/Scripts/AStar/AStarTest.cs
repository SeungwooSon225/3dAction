using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarTest : MonoBehaviour
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

    public Node FindPath(GameObject start, GameObject target)
    {
        _pathfinding.ResetGrid(Grid);
        _currentPath = _pathfinding.FindPath(new Vector2Int((int)Mathf.Round(start.transform.position.x), (int)Mathf.Round(start.transform.position.z)),
                                             new Vector2Int((int)target.transform.position.x, (int)target.transform.position.z));

        if (_currentPath == null) return null;

        return _currentPath[0];
    }

    //IEnumerator MoveCo()
    //{
    //    yield return null;

    //    float dst = 0;

    //    testGo.transform.position = StartPoint.transform.position;

    //    foreach (Node node in _currentPath)
    //    {
    //        dst += (node.Position - new Vector2Int((int)testGo.transform.position.x, (int)testGo.transform.position.z)).magnitude;
    //        dst += node.ZoneWeight;
    //        yield return new WaitForSeconds(1);

    //        testGo.transform.position = new Vector3(node.Position.x, 0, node.Position.y);
    //        Debug.Log($"{Grid[(int)node.Position.x, (int)node.Position.y].ZoneWeight}");
    //    }

    //    Debug.Log($"dst : {dst}");
    //}

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

        foreach (Transform child in gameObject.transform)
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

        _pathfinding = new AStarPathfinding(Grid);
    }
}
