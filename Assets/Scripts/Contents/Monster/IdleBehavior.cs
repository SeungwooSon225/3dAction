using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavior : IBehavior
{
    Transform _monster;
    Transform _player;
    Animator _animator;
    MonsterStat _monsterStat;
    bool _isDetect;
    bool _isDie;

    MonsterAI _monsterAI;

    public IdleBehavior(Transform monster, Transform player, Animator animator, MonsterAI monsterAI, MonsterStat monsterStat)
    {
        _monster = monster;
        _player = player;
        _animator = animator;
        _monsterAI = monsterAI;
        _monsterStat = monsterStat;
    }

    public BehaviorState Execute()
    {
        Debug.Log("Idle");

        if (!_isDie && _monsterStat.Hp == 0)
        {
            _isDie = true;
            _animator.SetTrigger("Die");
        }

        if (_isDie || _monsterAI.IsAttacking || _monsterAI.IsAttacked)
            return BehaviorState.Failure;

        _animator.speed = 1f;

        if (_isDetect)
            return BehaviorState.Success;

        if (Vector3.Distance(_monster.position, _player.position) < _monsterStat.DetectRange ||
            _monsterStat.Hp < _monsterStat.MaxHp - 0.1f)
        {
            Debug.Log("플레이어 탐지!");

            Managers.BGM.Play("Monster");

            _monsterStat.Target = _player;
            _isDetect = true;
            _monsterAI.MonsterUI.gameObject.SetActive(true);

            return BehaviorState.Success;
        }

        return BehaviorState.Failure;
    }
}
