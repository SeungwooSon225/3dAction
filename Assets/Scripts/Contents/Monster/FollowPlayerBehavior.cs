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

        if (_monsterStat.Target == null)
            _monsterStat.Target = _player;

        float distanceToTarget = Vector3.Distance(_monster.position, _monsterStat.Target.position);
        if (distanceToTarget > _monsterStat.StopDistance)
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

                if (_animator.GetBool("Fly Forward"))
                    _animator.SetBool("Fly Forward", false);

                return BehaviorState.Success;
            }

            return BehaviorState.Failure;
        }

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

        // 0.1초마다 실행
        if (_pathFindingTimer > 0.1f)
        {
            _pathFindingTimer = 0f;

            Node node = Managers.AStar.FindPath(_monster.gameObject, _player.gameObject);
            if (node == null)
                return false;

            // 부술 수 있는 장애물
            if (node.NodeType == NodeType.RemovableObstacle)
            {
                // 타겟을 장애물로 설정 -> 장애물에 접근한 후 장애물을 공격
                _monsterStat.Target = node.Object.transform;
                _moveDestination = node.Object.transform.position;
            }
            else
            {
                _monsterStat.Target = _player;
                _moveDestination = new Vector3(node.Position.x, 0, node.Position.y);
            }
        }

        _monster.rotation = Quaternion.Slerp(_monster.rotation, Quaternion.LookRotation(_monsterStat.Target.position - _monster.position), 10f * Time.deltaTime);
        _monster.position += (_moveDestination - _monster.position).normalized * Time.deltaTime * _monsterStat.MoveSpeed;

        return false;
    }

}
