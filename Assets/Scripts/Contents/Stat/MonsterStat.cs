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
    protected float _gold;

    public float DetectRange { get { return _detectRange; } set { _detectRange = value; } }
    public float StopDistance { get { return _stopDistance; } set { _stopDistance = value; } }
    public float AttackRange { get { return _attackRange; } set { _attackRange = value; } }
    public float AttackCoolTime { get { return _attackCoolTime; } set { _attackCoolTime = value; } }
    public float EscapeThreshold { get { return _escapeThreshold; } set { _escapeThreshold = value; } }
    public float Gold { get { return _gold; } set { _gold = value; } }


    void Start()
    {
        // To do
        _hp = 200f;
        _maxHp = 200f;
        _attack = 10f;
        _defense = 0f;
        _moveSpeed = 2f;
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
}
