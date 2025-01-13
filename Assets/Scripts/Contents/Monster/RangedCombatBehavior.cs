using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCombatBehavior : IBehavior
{
    Animator _animator;

    Transform _player;
    MonsterStat _monsterStat;

    //float _elapsedTime;
    

    public RangedCombatBehavior(Transform player, Animator animator, MonsterStat monsterStat)
    {
        _player = player;
        _monsterStat = monsterStat;

        _animator = animator;
    }

    public BehaviorState Execute()
    {
        Debug.Log("원거리 공격");
        _animator.SetTrigger("Cast Spell");

        return BehaviorState.Success;
    }
}
