using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    protected Dictionary<string, float> _staminaMpConsumption;

    [SerializeField]
    protected Define.PlayerClass _playerClass;

    [SerializeField]
    protected float _staminaMp;
    [SerializeField]
    protected float _maxStaminaMp;
    [SerializeField]
    protected float _staminaMpRecoverySpeed;

    protected int _exp;
    protected int _gold;

    [SerializeField]
    protected bool _isOnAttacked;

    protected bool _isOnAttackResist;


    public Dictionary<string, float> StaminaMpConsumption { get { return _staminaMpConsumption; } }

    public Define.PlayerClass PlayerClass { get { return _playerClass; } set { _playerClass = value; } }

    public float StaminaMp { get { return _staminaMp; } set { _staminaMp = value; } }
    public float MaxStaminaMp { get { return _maxStaminaMp; } set { _maxStaminaMp = value; } }
    public float StaminaMpRecoverySpeed { get { return _staminaMpRecoverySpeed; } set { _staminaMpRecoverySpeed = value; } }

    public int Exp { get { return _exp; } set { _hp = value; } }
    public int Gold { get { return _gold; } set { _hp = value; } }

    public bool IsOnAttacked { get { return _isOnAttacked; } set { _isOnAttacked = value; } }
    public bool IsOnAttackResist { get { return _isOnAttackResist; } set { _isOnAttackResist = value; } }

    bool _isDown;
    public bool IsDown { get { return _isDown; } set { _isDown = value; } }
    Animator _animator;

    protected virtual void Init()
    {
        Managers.Data.InitPlayerStat(_playerClass);

        //_isAttackable = true;
        _attackWeight = new Dictionary<string, Define.AttackWeight>();
        _staminaMpConsumption = new Dictionary<string, float>();
        _animator = gameObject.GetComponent<Animator>();

        _isOnAttackResist = false;

        SetStat(1);
    }


    public void SetStat(int level)
    {
        Data.Stat stat = Managers.Data.StatDict[level];

        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
        _defense = stat.defense;
        _moveSpeed = stat.moveSpeed;
        _staminaMp = stat.maxStaminaMP;
        _maxStaminaMp = stat.maxStaminaMP;
        _staminaMpRecoverySpeed = stat.staminaMpRecoverySpeed;

        _staminaMpConsumption.Add("Dodge", stat.dodgeConsumption);
        _staminaMpConsumption.Add("BasicAttack", stat.basicAttackConsumption);
        _staminaMpConsumption.Add("ChargeAttack", stat.chargeAttackConsumption);
        _staminaMpConsumption.Add("SkillE", stat.skillEConsumption);
        _staminaMpConsumption.Add("SkillR", stat.skillRConsumption);
    }


    public override void OnAttacked(Attack attacker)
    {
        if (_isDead || _isDown) return;

        float damage = Mathf.Max(0, attacker.Damage - Defense);

        Hp -= damage;

        if (Hp <= 0)
        {
            Hp = 0;
            OnDead(attacker);

            return;
        }

        if (attacker.AttackType == AttackType.Basic && (IsOnAttackResist || IsOnAttacked))
            return;

        StartCoroutine(OnAttackedCo(attacker));
    }

    IEnumerator OnAttackedCo(Attack attacker)
    {
        IsOnAttacked = true;

        if (attacker.AttackType == AttackType.Basic)
            _animator.SetTrigger("OnAttacked");
        else
        {
            _isDown = true;
            _animator.SetTrigger("OnAttackedHeavy");
        }

        _animator.SetBool("IsAttacking", false);
        _animator.SetBool("IsDodging", false);
        _animator.ResetTrigger("LeftShortClick");
        _animator.ResetTrigger("LeftLongClick");
        _animator.ResetTrigger("SkillE");
        _animator.ResetTrigger("SkillR");

        yield return new WaitForSeconds(0.5f);

        IsOnAttacked = false;
    }

    protected override void OnDead(Attack attacker)
    {
        _isDead = true;
        _animator.SetTrigger("Die");
    }

    void SetOnAttackedResistTrue()
    {
        _isOnAttackResist = true;
    }

    void SetOnAttackedResistFalse()
    {
        _isOnAttackResist = false;
    }

    void SetIsDownFalse()
    {
        _isDown = false;
    }
}
