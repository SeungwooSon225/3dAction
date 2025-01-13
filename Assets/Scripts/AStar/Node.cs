using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2Int Position;
    public bool IsWalkable;
    public float GCost;             // ���� ��忡�� ���� �������� ���
    public float HCost;             // ���� ��忡�� ��ǥ �������� ���� ���
    public float Fcost => GCost + HCost;
    public float ZoneWeight;
    public Node Parent;

    public Node(Vector2Int position, bool isWalkable)
    {
        Position = position;
        IsWalkable = isWalkable;
        GCost = 10000; // �ʱ� ���
        HCost = 0;
        ZoneWeight = 0;
        Parent = null;
    }
}
