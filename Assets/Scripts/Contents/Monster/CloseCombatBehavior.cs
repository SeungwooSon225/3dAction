using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCombatBehavior : IBehavior
{
    Animator _animator;

    Transform _monster;
    Transform _player;
    MonsterStat _monsterStat;

    float _elapsedTime;


    public CloseCombatBehavior(Transform monster, Transform player, Animator animator, MonsterStat monsterStat)
    {
        _monster = monster;
        _player = player;
        _monsterStat = monsterStat;
        _animator = animator;
        _elapsedTime = _monsterStat.AttackCoolTime;
    }

    public BehaviorState Execute()
    {
        if (Vector3.Distance(_monster.position, _player.position) < _monsterStat.AttackRange)
        {
            _monster.rotation = Quaternion.Slerp(_monster.rotation, Quaternion.LookRotation(_player.position - _monster.position), 10f * Time.deltaTime);

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _monsterStat.AttackCoolTime)
            {
                //Debug.Log("근접 공격");

                float random = Random.Range(0f, 1f);

                if (random < 10.1f)
                {
                    _animator.SetTrigger("SliceAttack");
                    //_monsterStat.SetAttackDamage("SliceAttack");
                }
                else
                {
                    _animator.SetTrigger("PunchAttack");
                    //_monsterStat.SetAttackDamage("PunchAttack");
                }


                _elapsedTime = 0f;

                return BehaviorState.Success;
            }

            return BehaviorState.Running;
        }

        _elapsedTime = _monsterStat.AttackCoolTime;

        return BehaviorState.Failure;
    }
}
