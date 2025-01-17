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
        if (!_isDie && _monsterStat.Hp == 0)
        {
            _isDie = true;
            _animator.SetTrigger("Die");       
        }

        if(_isDie || _monsterAI.IsAttacking)
            return BehaviorState.Failure;

        if (_isDetect)
            return BehaviorState.Success;

        if (Vector3.Distance(_monster.position, _player.position) < _monsterStat.DetectRange)
        {
            Debug.Log("플레이어 탐지!");
            _isDetect = true;

            UI_MonsterStat ui = Managers.UI.ShowUI<UI_MonsterStat>();
            ui.SetName(_monster.name);
            ui.MonsterStat = _monsterStat;

            return BehaviorState.Success;
        }

        return BehaviorState.Failure;
    }
}
