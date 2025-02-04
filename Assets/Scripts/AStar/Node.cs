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
    public float GCost;             // ���� ��忡�� ���� �������� ���
    public float HCost;             // ���� ��忡�� ��ǥ �������� ���� ���
    public float Fcost => GCost + HCost;
    public float ZoneWeight;
    public Node Parent;
    public GameObject Object;

    public Node(Vector2Int position, bool isWalkable)
    {
        NodeType = NodeType.None;
        Position = position;
        IsWalkable = isWalkable;
        GCost = 10000; // �ʱ� ���
        HCost = 0;
        ZoneWeight = 0;
        Parent = null;
        Object = null;
    }
}
