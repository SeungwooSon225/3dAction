using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected Define.PlayerClass _playerClass;
    [SerializeField]
    protected int _level;
    [SerializeField]
    protected float _staminaMp;
    [SerializeField]
    protected float _maxStaminaMp;
    [SerializeField]
    protected float _staminaMpRecoverySpeed;
    [SerializeField]
    protected int _gold;

    protected Dictionary<string, float> _staminaMpConsumption;

    [SerializeField]
    protected bool _isOnAttacked;

    protected bool _isOnAttackResist;


    public Define.PlayerClass PlayerClass { get { return _playerClass; } set { _playerClass = value; } }
    public int Level { get { return _level; } set { _level = value; } }
    public float StaminaMp { get { return _staminaMp; } set { _staminaMp = value; } }
    public float MaxStaminaMp { get { return _maxStaminaMp; } set { _maxStaminaMp = value; } }
    public float StaminaMpRecoverySpeed { get { return _staminaMpRecoverySpeed; } set { _staminaMpRecoverySpeed = value; } }
    public int Gold { get { return _gold; } 
        set 
        { 
            _gold = value;
            Managers.UI.UI_PlayerStat.UpdateGold();
        } 
    }

    public Dictionary<string, float> StaminaMpConsumption { get { return _staminaMpConsumption; } }

    public bool IsOnAttacked { get { return _isOnAttacked; } set { _isOnAttacked = value; } }
    public bool IsOnAttackResist { get { return _isOnAttackResist; } set { _isOnAttackResist = value; } }

    protected bool _isDown;
    public bool IsDown { get { return _isDown; } set { _isDown = value; } }

    Animator _animator;


    protected override void Init()
    {
        _collider = GetComponent<Collider>();

        Managers.Data.InitPlayerStat(_playerClass);

        _attackWeight = new Dictionary<string, Define.AttackWeight>();
        _staminaMpConsumption = new Dictionary<string, float>();
        _animator = gameObject.GetComponent<Animator>();

        _isOnAttackResist = false;

        _gold = 1000;

        SetStat(1);
        SetAttackWeight();
    }


    public void SetStat(int level)
    {
        _level = level;

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

    public void SetOnAttackedResistTrue()
    {
        _isOnAttackResist = true;
    }

    public void SetOnAttackedResistFalse()
    {
        _isOnAttackResist = false;
    }

    void SetIsDownFalse()
    {
        _isDown = false;
    }

    private void SpendStaminaMp(string attackName)
    {
        if (_staminaMp >= _staminaMpConsumption[attackName])
        {
            _staminaMp -=_staminaMpConsumption[attackName];
        }
    }
}
