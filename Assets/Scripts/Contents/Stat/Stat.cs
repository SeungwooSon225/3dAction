using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stat : MonoBehaviour
{
    [SerializeField]
    protected float _hp;
    [SerializeField]
    protected float _maxHp;
    [SerializeField]
    protected float _attack;
    [SerializeField]
    protected float _defense;
    [SerializeField]
    protected float _moveSpeed;
    [SerializeField]
    protected bool _isDead;

    [SerializeField]
    protected Transform _target;

    protected Dictionary<string, Define.AttackWeight> _attackWeight = new Dictionary<string, Define.AttackWeight>();

    public float Hp { get { return _hp; } set { _hp = value; } }
    public float MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public float Attack { get { return _attack; } set { _attack = value; } }
    public float Defense { get { return _defense; } set { _defense = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public bool IsDead { get { return _isDead; } set { _isDead = value; } }

    public Transform Target { get { return _target; } set { _target = value; } }

    public Dictionary<string, Define.AttackWeight> AttackWeight { get { return _attackWeight; } set { _attackWeight = value; } }

    protected Collider _collider;


    void Start()
    {
        Init();
    }


    protected virtual void Init() { }

    public abstract void OnAttacked(Attack attacker);

    protected abstract void OnDead(Attack attacker);

    private void SetAttackableTrue()
    {
        _collider.enabled = true;
    }

    private void SetAttackableFalse()
    {
        _collider.enabled = false;
    }

    public virtual void EnableAttack(string name)
    {
        _attackWeight[name].Attack.Damage = _attack * _attackWeight[name].Weight;
        _attackWeight[name].Attack.GetComponent<Collider>().enabled = true;
    }

    public virtual void DisableAttackCollider(string name)
    {
        _attackWeight[name].Attack.GetComponent<Collider>().enabled = false;
    }

    protected virtual void SetAttackWeight() { }
}
