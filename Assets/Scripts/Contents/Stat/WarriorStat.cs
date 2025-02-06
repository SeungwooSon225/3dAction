using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorStat : PlayerStat
{
    [SerializeField]
    Attack _weapon;


    protected override void Init()
    {
        _playerClass = Define.PlayerClass.Warrior;

        base.Init();

        SetAttackWeight();
    }


    protected override void SetAttackWeight()
    {
        Data.Stat stat = Managers.Data.StatDict[1];

        Define.AttackWeight basicComboOne = new Define.AttackWeight(_weapon, stat.basicComboOneWeight);
        AttackWeight.Add("BasicComboOne", basicComboOne);

        Define.AttackWeight basicComboTwo = new Define.AttackWeight(_weapon, stat.basicComboTwoWeight);
        AttackWeight.Add("BasicComboTwo", basicComboTwo);

        Define.AttackWeight basicComboThree = new Define.AttackWeight(_weapon, stat.basicComboThreeWeight);
        AttackWeight.Add("BasicComboThree", basicComboThree);

        Define.AttackWeight skillE = new Define.AttackWeight(null, stat.skillEWeight);
        AttackWeight.Add("Warrior@SkillE", skillE);

        Define.AttackWeight skillR = new Define.AttackWeight(null, stat.skillRWeight);
        AttackWeight.Add("Warrior@SkillR", skillR);

        //Debug.Log(AttackWeight["BasicComboOne"].Weight);
    }

    public override void OnAttacked(Attack attacker)
    {
        DisenableAttack("BasicComboOne");

        base.OnAttacked(attacker);
    }
}
