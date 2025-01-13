using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    // 애니메이션 -> FSM ?

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
        float distance = direction.magnitude; // 시작 지점과 끝 지점 사이의 거리

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, direction.normalized, out RaycastHit hit, distance))
        {
            // 장애물 있을 때
            if (hit.collider.CompareTag("Obstacle"))
            {
                Debug.Log($"Obstacle 발견");

                Node node = AStarTest.FindPath(gameObject, Player);

                if (node != null)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(node.Position.x, 0, node.Position.y) - transform.position), 10f * Time.deltaTime);
                    transform.position += transform.forward * Time.deltaTime * 3f;
                }
            }
            // 장애물 없을 때
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Player.transform.position - transform.position), 10f * Time.deltaTime);

                if ((transform.position - Player.transform.position).magnitude > 2f)
                    transform.position += transform.forward * Time.deltaTime * 3f;
            }
        }
    }
}
