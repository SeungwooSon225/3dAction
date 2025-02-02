using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCombatBehavior : IBehavior
{
    Animator _animator;

    Transform _monster;
    Transform _player;
    MonsterStat _monsterStat;

    MonsterAI _monsterAI;

    float _elapsedTime;


    public CloseCombatBehavior(Transform monster, Transform player, Animator animator, MonsterAI monsterAI, MonsterStat monsterStat)
    {
        _monster = monster;
        _player = player;
        _monsterStat = monsterStat;
        _animator = animator;
        _elapsedTime = _monsterStat.AttackCoolTime;
        _monsterAI = monsterAI;
    }

    public BehaviorState Execute()
    {
        _monster.rotation = Quaternion.Slerp(_monster.rotation, Quaternion.LookRotation(_monsterStat.Target.position - _monster.position), 10f * Time.deltaTime);

        if (Vector3.Distance(_monster.position, _monsterStat.Target.position) < _monsterStat.AttackRange)
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _monsterStat.AttackCoolTime)
            {
                if (_monsterStat.Target.tag == "RemovableObstacle")
                {
                    _animator.SetTrigger("ShockwaveAttack");
                }
                else
                {
                    //Debug.Log("근접 공격");
                    _monsterAI.IsAttacking = true;
                    float random = Random.Range(0f, 1f);

                    if (random < 10.2f)
                    {
                        _animator.SetTrigger("ShockwaveAttack");
                        //_monsterStat.SetAttackDamage("SliceAttack");
                    }
                    else if (random < 0.6f)
                    {
                        _animator.SetTrigger("SliceAttack");
                        //_monsterStat.SetAttackDamage("SliceAttack");
                    }
                    else
                    {
                        _animator.SetTrigger("PunchAttack");
                        //_monsterStat.SetAttackDamage("PunchAttack");
                    }
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
