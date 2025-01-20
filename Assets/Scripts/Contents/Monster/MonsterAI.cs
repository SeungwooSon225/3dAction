using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    IBehavior _behaviorTree;

    Animator _animator;
    MonsterStat _monsterStat;

    public Transform _player;
    public Attack RightHandAttack;
    public Attack LeftHandAttack;

    public bool IsAttacking;


    void Start()
    {    
        Init();
    }

    private void Init()
    {
        Debug.Log("Monster Init");
        _player = Managers.Game.Player.transform;

        _animator = gameObject.GetComponent<Animator>();
        _monsterStat = gameObject.GetComponent<MonsterStat>();

        List<IBehavior> IBehaviorTreeList = new List<IBehavior>();

        List<IBehavior> IBehaviorMoveList = new List<IBehavior>();
        IBehaviorMoveList.Add(new IdleBehavior(transform, _player, _animator, this, _monsterStat));
        IBehaviorMoveList.Add(new FollowPlayerBehavior(transform, _player, _animator, _monsterStat));
        Sequence moveSequence = new Sequence(IBehaviorMoveList);

        List<IBehavior> IBehaviorAttackList = new List<IBehavior>();
        IBehaviorAttackList.Add(new SpecialAttackBehavor(transform, _player, _animator, this, _monsterStat));
        IBehaviorAttackList.Add(new CloseCombatBehavior(transform, _player, _animator, this, _monsterStat));
        IBehaviorAttackList.Add(new RangedCombatBehavior(_player, _animator, this, _monsterStat));
        Selector attackSelector = new Selector(IBehaviorAttackList);

        IBehaviorTreeList.Add(moveSequence);
        IBehaviorTreeList.Add(attackSelector);
        _behaviorTree = new Sequence(IBehaviorTreeList);

        // To do
        if (_monsterStat.AttackWeight == null)
            _monsterStat.AttackWeight = new Dictionary<string, Define.AttackWeight>();

        RightHandAttack.IsPlayer = false;
        Define.AttackWeight punchAttack = new Define.AttackWeight(RightHandAttack, 1.0f);
        _monsterStat.AttackWeight.Add("PunchAttack", punchAttack);
        //Debug.Log("pa: " + _monsterStat.AttackWeight["PunchAttack"].Weight);

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

    private void EndAttack()
    {
        IsAttacking = false;
    }

    private void SummonSnowStorm()
    {
        GameObject snowStorm = Managers.Resource.Instantiate("CrystalGuardian/snowStorm");
        snowStorm.transform.position = gameObject.transform.position + Vector3.up * 9f;
        snowStorm.GetComponent<SnowStorm>().CastStorm(_player, _animator);
    }
}
