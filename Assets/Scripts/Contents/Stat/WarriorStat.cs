using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorStat : PlayerStat
{
    protected override void SetAttackWeight()
    {
        Data.Stat stat = Managers.Data.StatDict[1];
        Attack sword = Util.FindDeepChild(transform, "Sword").GetComponent<Attack>(); ;

        Define.AttackWeight basicComboOne = new Define.AttackWeight(sword, stat.basicComboOneWeight);
        AttackWeight.Add("BasicComboOne", basicComboOne);

        Define.AttackWeight basicComboTwo = new Define.AttackWeight(sword, stat.basicComboTwoWeight);
        AttackWeight.Add("BasicComboTwo", basicComboTwo);

        Define.AttackWeight basicComboThree = new Define.AttackWeight(sword, stat.basicComboThreeWeight);
        AttackWeight.Add("BasicComboThree", basicComboThree);

        Define.AttackWeight skillE = new Define.AttackWeight(null, stat.skillEWeight);
        AttackWeight.Add("Warrior@SkillE", skillE);

        Define.AttackWeight skillR = new Define.AttackWeight(null, stat.skillRWeight);
        AttackWeight.Add("Warrior@SkillR", skillR);
    }

    public override void OnAttacked(Attack attacker)
    {
        DisableAttackCollider("BasicComboOne");

        base.OnAttacked(attacker);
    }
}
