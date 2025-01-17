using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCombatBehavior : IBehavior
{
    Animator _animator;

    Transform _player;
    MonsterStat _monsterStat;
    MonsterAI _monsterAI;

    //float _elapsedTime;


    public RangedCombatBehavior(Transform player, Animator animator, MonsterAI monsterAI, MonsterStat monsterStat)
    {
        _player = player;
        _monsterStat = monsterStat;

        _monsterAI = monsterAI;
        _animator = animator;
    }

    public BehaviorState Execute()
    {
        Debug.Log("원거리 공격");
        _monsterAI.IsAttacking = true;
        _animator.SetTrigger("Cast Spell");
        //_animator.SetBool("SnowStorm", true);

        return BehaviorState.Success;
    }
}
