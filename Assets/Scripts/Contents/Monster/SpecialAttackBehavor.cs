using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackBehavor : IBehavior
{
    Animator _animator;

    Transform _monster;
    Transform _player;
    MonsterStat _monsterStat;

    MonsterAI _monsterAI;

    int _excutedCount = 0;

    public SpecialAttackBehavor(Transform monster, Transform player, Animator animator, MonsterAI monsterAI, MonsterStat monsterStat)
    {
        _monster = monster;
        _player = player;
        _monsterStat = monsterStat;
        _animator = animator;
        _monsterAI = monsterAI;
    }

    public BehaviorState Execute()
    {
        if (_monsterStat.Hp / _monsterStat.MaxHp < 0.7f && _excutedCount == 0)
        {
            Debug.Log("±Ã±Ø±â");
            _monsterAI.IsAttacking = true;
            _excutedCount = 1;
            _animator.SetBool("SnowStorm", true);

            return BehaviorState.Success;
        }

        if (_monsterStat.Hp / _monsterStat.MaxHp < 0.3f && _excutedCount == 1)
        {
            Debug.Log("±Ã±Ø±â");
            _monsterAI.IsAttacking = true;
            _excutedCount = 2;
            _animator.SetBool("SnowStorm", true);

            return BehaviorState.Success;
        }

        return BehaviorState.Failure; 
    }
}
