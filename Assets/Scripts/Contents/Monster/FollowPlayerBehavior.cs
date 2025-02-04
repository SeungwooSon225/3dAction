using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerBehavior : IBehavior
{
    Animator _animator;

    Transform _monster;
    Transform _player;
    MonsterStat _monsterStat;

    float _elapsedTime;

    public FollowPlayerBehavior(Transform monster, Transform player, Animator animator, MonsterStat monsterStat)
    {
        _monster = monster;
        _player = player;
        _monsterStat = monsterStat;
        _animator = animator;
    }


    public BehaviorState Execute()
    {
        //_monster.rotation = Quaternion.Slerp(_monster.rotation, Quaternion.LookRotation(_player.position - _monster.position), 10f * Time.deltaTime);

        if (_animator.GetBool("IsAttacking"))
        {
            return BehaviorState.Failure;
        }

        float distanceToPlayer = Vector3.Distance(_monster.position, _player.position);

        if (distanceToPlayer > _monsterStat.StopDistance)
        {
            //Debug.Log("추적");
            // 부술 수 있는 장애물일 경우
            if(TracePlayer())
                return BehaviorState.Success;

            if (!_animator.GetBool("Fly Forward"))
                _animator.SetBool("Fly Forward", true);

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > _monsterStat.EscapeThreshold)
            {
                _elapsedTime = 0f;
                //Debug.Log("멀어");

                if (_animator.GetBool("Fly Forward"))
                    _animator.SetBool("Fly Forward", false);

                return BehaviorState.Success;
            }

            return BehaviorState.Failure;
        }

        _elapsedTime = 0f;

        //Debug.Log("공격");

        if (_animator.GetBool("Fly Forward"))
            _animator.SetBool("Fly Forward", false);
        return BehaviorState.Success;
    }


    float _pathFindingTimer = 0.25f;
    Vector3 _moveDestination;

    bool TracePlayer()
    {
        _pathFindingTimer += Time.deltaTime;

        // 0.25초마다 실행
        if (_pathFindingTimer > 0.25f)
        {
            //Debug.Log("PathFinding");

            _pathFindingTimer = 0f;

            // 몬스터와 플레이어 사이에 장애물이 있는지 검사: Y -> AStarPathFinding 실행, N -> 직선 거리로 이동
            Vector3 direction = _player.position - _monster.position;
            float distance = direction.magnitude; 
            if (Physics.Raycast(_monster.position + Vector3.up * 0.5f, direction.normalized, out RaycastHit hit, distance))
            {
                // 장애물 있을 때
                if (hit.collider.CompareTag("Obstacle") || hit.collider.CompareTag("RemovableObstacle") || hit.collider.CompareTag("MonsterObstacle"))
                {
                    //Debug.Log($"Obstacle 발견");
                    Node node = Managers.AStar.FindPath(_monster.gameObject, _player.gameObject);

                    if (node == null)
                        return false;

                    // 부술 수 있는 장애물
                    if (Mathf.Abs(node.ZoneWeight - 10.123f) < 0.0001f)
                    {
                        _monsterStat.Target = GameObject.Find("Fence").transform;

                        return true;

                    }
                    else
                    {
                        _monsterStat.Target = _player;

                        _moveDestination = new Vector3(node.Position.x, 0, node.Position.y);
                    }

                }
                // 장애물 없을 때
                else
                {
                    _monsterStat.Target = _player;

                    _moveDestination = _player.position;

                    Debug.Log(_moveDestination);

                    //_monster.rotation = Quaternion.Slerp(_monster.rotation, Quaternion.LookRotation(_player.position - _monster.position), 10f * Time.deltaTime);

                    //if ((_monster.position - _player.position).magnitude > 2f)
                    //    _monster.position += _monster.forward * Time.deltaTime * _monsterStat.MoveSpeed;
                }
            }
        }

        _monster.rotation = Quaternion.Slerp(_monster.rotation, Quaternion.LookRotation(new Vector3(_moveDestination.x, 0, _moveDestination.z) - _monster.position), 10f * Time.deltaTime);
        _monster.position += _monster.forward * Time.deltaTime * _monsterStat.MoveSpeed;

        return false;
    }

}
