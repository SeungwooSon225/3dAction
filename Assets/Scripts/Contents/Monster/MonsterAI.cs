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
    public Attack LeftHandAttack;

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

        LeftHandAttack.IsPlayer = false;
        Define.AttackWeight sliceAttack = new Define.AttackWeight(LeftHandAttack, 1.0f);
        _monsterStat.AttackWeight.Add("SliceAttack", sliceAttack);
    }


    void Update()
    {
        _behaviorTree.Execute();
    }

    private void ShootProjectiles()
    {
        GameObject projectile1 = Managers.Resource.Instantiate($"Projectiles/CrystalGuardian@Missile");
        projectile1.GetComponent<Projectile>().HalfParabolicShoot(_monsterStat, new Vector3(2f, 1f, 0f));

        GameObject projectile2 = Managers.Resource.Instantiate($"Projectiles/CrystalGuardian@Missile");
        projectile2.GetComponent<Projectile>().HalfParabolicShoot(_monsterStat, new Vector3(0f, 1f, 0f));

        GameObject projectile3 = Managers.Resource.Instantiate($"Projectiles/CrystalGuardian@Missile");
        projectile3.GetComponent<Projectile>().HalfParabolicShoot(_monsterStat, new Vector3(-2f, 1f, 0f));
    }
}
