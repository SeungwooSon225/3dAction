using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum NodeType
{ 
    None,
    Obstacle,
    RemovableObstacle,
}

public class Node
{
    public NodeType NodeType;
    public Vector2Int Position;
    public bool IsWalkable;
    public float GCost;             // 시작 노드에서 현재 노드까지의 비용
    public float HCost;             // 현재 노드에서 목표 노드까지의 예상 비용
    public float Fcost => GCost + HCost;
    public float ZoneWeight;
    public Node Parent;
    public GameObject Object;

    public Node(Vector2Int position, bool isWalkable)
    {
        NodeType = NodeType.None;
        Position = position;
        IsWalkable = isWalkable;
        GCost = 10000; // 초기 비용
        HCost = 0;
        ZoneWeight = 0;
        Parent = null;
        Object = null;
    }
}
