using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStat : PlayerStat
{
    [SerializeField]
    protected int _mp;
    [SerializeField]
    protected int _maxMp;

    public int Mp { get { return _mp; } set { _hp = value; } }
    public int MaxMp { get { return _maxMp; } set { _hp = value; } }

    protected override void SetStat(int level)
    {
        
    }

    void Start()
    {
        _hp = 100;
        _maxHp = 100;
        _attack = 10;
        _defense = 10;
        _moveSpeed = 5;
        _mp = 100;
        _maxMp = 100;
    }
}
