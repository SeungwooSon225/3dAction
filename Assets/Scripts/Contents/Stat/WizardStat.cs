using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStat : PlayerStat
{
    [SerializeField]
    Attack _weapon;

    void Start()
    {
        Init();
    }

    protected override void Init()
    {
        _playerClass = Define.PlayerClass.Wizard;

        base.Init();

        SetAttackWeight();
    }
}
