using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    IBehavior _behaviorTree;

    Animator _animator;
    MonsterStat _monsterStat;

    public Transform Player;
    public Attack RightHandAttack;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        _animator = gameObject.GetComponent<Animator>();
        _monsterStat = gameObject.GetComponent<MonsterStat>();

        List<IBehavior> IBehaviorTreeList = new List<IBehavior>();

        List<IBehavior> IBehaviorMoveList = new List<IBehavior>();
        IBehaviorMoveList.Add(new IdleBehavior(transform, Player, _animator, _monsterStat));
        IBehaviorMoveList.Add(new FollowPlayerBehavior(transform, Player, _animator, _monsterStat));
        Sequence moveSequence = new Sequence(IBehaviorMoveList);

        List<IBehavior> IBehaviorAttackList = new List<IBehavior>();
        IBehaviorAttackList.Add(new CloseCombatBehavior(transform, Player, _animator, _monsterStat));
        IBehaviorAttackList.Add(new RangedCombatBehavior(Player, _animator, _monsterStat));
        Selector attackSelector = new Selector(IBehaviorAttackList);

        IBehaviorTreeList.Add(moveSequence);
        IBehaviorTreeList.Add(attackSelector);
        _behaviorTree = new Sequence(IBehaviorTreeList);

        RightHandAttack.IsPlayer = false;
        Define.AttackWeight punchAttack = new Define.AttackWeight(RightHandAttack, 1.0f);
        _monsterStat.AttackWeight.Add("PunchAttack", punchAttack);
    }


    void Update()
    {
        _behaviorTree.Execute();
    }
}
