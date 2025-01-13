using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    // �ִϸ��̼� -> FSM ?

    public GameObject Player;
    public AStarTest AStarTest;

    void Start()
    {
        
    }

    void Update()
    {
        TracePlayer();
    }


    void TracePlayer()
    {
        Vector3 direction = Player.transform.position - transform.position;
        float distance = direction.magnitude; // ���� ������ �� ���� ������ �Ÿ�

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, direction.normalized, out RaycastHit hit, distance))
        {
            // ��ֹ� ���� ��
            if (hit.collider.CompareTag("Obstacle"))
            {
                Debug.Log($"Obstacle �߰�");

                Node node = AStarTest.FindPath(gameObject, Player);

                if (node != null)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(node.Position.x, 0, node.Position.y) - transform.position), 10f * Time.deltaTime);
                    transform.position += transform.forward * Time.deltaTime * 3f;
                }
            }
            // ��ֹ� ���� ��
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Player.transform.position - transform.position), 10f * Time.deltaTime);

                if ((transform.position - Player.transform.position).magnitude > 2f)
                    transform.position += transform.forward * Time.deltaTime * 3f;
            }
        }
    }
}
