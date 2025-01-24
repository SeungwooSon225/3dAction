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

        Define.AttackWeight fireBall = new Define.AttackWeight(null, 1f);
        AttackWeight.Add("Wizard@FireBall", fireBall);

        Define.AttackWeight fireBall2 = new Define.AttackWeight(null, 1f);
        AttackWeight.Add("Wizard@FireBall2", fireBall2);

        Define.AttackWeight chargeAttack = new Define.AttackWeight(null, 1.5f);
        AttackWeight.Add("Wizard@ChargeAttack", chargeAttack);

        Define.AttackWeight skillEAttack = new Define.AttackWeight(null, 1.5f);
        AttackWeight.Add("Wizard@SkillE", skillEAttack);
    }
}
