using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStat : PlayerStat
{
    protected override void SetAttackWeight()
    {
        Data.Stat stat = Managers.Data.StatDict[1];

        Define.AttackWeight fireBall = new Define.AttackWeight(null, stat.basicComboOneWeight);
        AttackWeight.Add("Wizard@FireBall", fireBall);

        Define.AttackWeight fireBall2 = new Define.AttackWeight(null, stat.basicComboOneWeight);
        AttackWeight.Add("Wizard@FireBall2", fireBall2);

        Define.AttackWeight chargeAttack = new Define.AttackWeight(null, stat.basicComboThreeWeight);
        AttackWeight.Add("Wizard@ChargeAttack", chargeAttack);

        Define.AttackWeight skillEAttack = new Define.AttackWeight(null, stat.skillEWeight);
        AttackWeight.Add("Wizard@SkillE", skillEAttack);

        Define.AttackWeight skillRAttack = new Define.AttackWeight(null, stat.skillRWeight);
        AttackWeight.Add("Wizard@SkillR", skillRAttack);
    }
}
