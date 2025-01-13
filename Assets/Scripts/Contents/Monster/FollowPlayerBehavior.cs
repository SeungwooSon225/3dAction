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
            Debug.Log("추적");
            TracePlayer();

            if(!_animator.GetBool("Fly Forward"))
                _animator.SetBool("Fly Forward", true);

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > _monsterStat.EscapeThreshold)
            {
                _elapsedTime = 0f;
                Debug.Log("멀어");

                if (_animator.GetBool("Fly Forward"))
                    _animator.SetBool("Fly Forward", false);
                return BehaviorState.Success;
            }

            return BehaviorState.Failure;
        }

        _elapsedTime = 0f;

        Debug.Log("공격");

        if (_animator.GetBool("Fly Forward"))
            _animator.SetBool("Fly Forward", false);
        return BehaviorState.Success;
    }

    void TracePlayer()
    {
        Vector3 direction = _player.position - _monster.position;
        float distance = direction.magnitude; // 시작 지점과 끝 지점 사이의 거리

        if (Physics.Raycast(_monster.position + Vector3.up * 0.5f, direction.normalized, out RaycastHit hit, distance))
        {
            // 장애물 있을 때
            if (hit.collider.CompareTag("Obstacle"))
            {
                Debug.Log($"Obstacle 발견");

                Node node = Managers.AStar.FindPath(_monster.gameObject, _player.gameObject);

                if (node != null)
                {
                    _monster.rotation = Quaternion.Slerp(_monster.rotation, Quaternion.LookRotation(new Vector3(node.Position.x, 0, node.Position.y) - _monster.position), 10f * Time.deltaTime);
                    _monster.position += _monster.forward * Time.deltaTime * _monsterStat.MoveSpeed;
                }
            }
            // 장애물 없을 때
            else
            {
                _monster.rotation = Quaternion.Slerp(_monster.rotation, Quaternion.LookRotation(_player.position - _monster.position), 10f * Time.deltaTime);

                if ((_monster.position - _player.position).magnitude > 2f)
                    _monster.position += _monster.forward * Time.deltaTime * _monsterStat.MoveSpeed;
            }
        }
    }
}
