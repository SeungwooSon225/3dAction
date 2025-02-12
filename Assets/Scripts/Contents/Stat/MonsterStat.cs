using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : Stat
{
    [SerializeField]
    protected float _detectRange;
    [SerializeField]
    protected float _stopDistance;
    [SerializeField]
    protected float _attackRange;
    [SerializeField]
    protected float _attackCoolTime;
    [SerializeField]
    protected float _escapeThreshold;
    [SerializeField]
    protected int _dropGold;

    public float DetectRange { get { return _detectRange; } set { _detectRange = value; } }
    public float StopDistance { get { return _stopDistance; } set { _stopDistance = value; } }
    public float AttackRange { get { return _attackRange; } set { _attackRange = value; } }
    public float AttackCoolTime { get { return _attackCoolTime; } set { _attackCoolTime = value; } }
    public float EscapeThreshold { get { return _escapeThreshold; } set { _escapeThreshold = value; } }
    public int DropGold { get { return _dropGold; } set { _dropGold = value; } }


    Animator _animator;
    MonsterAI _monsterAI;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _monsterAI = GetComponent<MonsterAI>();

        // To do
        _hp = 50f;
        _maxHp = 50f;
        _attack = 10f;
        _defense = 0f;
        _moveSpeed = 2f;
        _dropGold = 100000;
        //_isAttackable = true;

        _detectRange = 10f;
        _stopDistance = 5f;
        _attackRange = 5f;
        _attackCoolTime = 2f;
        _escapeThreshold = 3f;

        if(AttackWeight == null)
            AttackWeight = new Dictionary<string, Define.AttackWeight>();

        Define.AttackWeight shockwaveAttack = new Define.AttackWeight(null, 1.5f);
        AttackWeight.Add("CrystalGuardian@ShockwaveAttack", shockwaveAttack);
        Define.AttackWeight missile = new Define.AttackWeight(null, 0.5f);
        AttackWeight.Add("CrystalGuardian@Missile", missile);
    }


    public override void OnAttacked(Attack attacker)
    {
        float damage = Mathf.Max(0, attacker.Damage - Defense);

        Hp -= damage;

        if (Hp <= 0)
        {
            Hp = 0;
            OnDead(attacker);

            //IsAttackable = false;
        }
        else if (attacker.AttackType == AttackType.Heavy)
        {
            _monsterAI.IsAttacked = true;
            _monsterAI.IsAttacking = false;
            _animator.SetTrigger("OnAttacked");
            _animator.SetBool("Fly Forward", false);
        }
    }

    protected override void OnDead(Attack attacker)
    {     
        Managers.Game.Player.GetComponent<PlayerStat>().Gold += _dropGold;
        _dropGold = 0;
        gameObject.GetComponent<Collider>().enabled = false;
        Managers.Resource.Destroy(_monsterAI.MonsterUI.gameObject);
    }
}
