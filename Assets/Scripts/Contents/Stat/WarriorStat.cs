using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorStat : PlayerStat
{
    [SerializeField]
    Attack _weapon;

    void Start()
    { 
        _isAttackable = true;

        AttackWeight = new Dictionary<string, Define.AttackWeight>();

        SetStat(1);
        SetStaminaMpConsumption(1);
        SetAttackWeight();
    }

    protected override void SetStat(int level)
    {
        Dictionary<int, Data.WarriorStat> dict = Managers.Data.WarriorStatDict;
        Data.WarriorStat stat = dict[level];

        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
        _defense = stat.defense;
        _moveSpeed = stat.moveSpeed;
        _staminaMp = stat.maxStaminaMP;
        _maxStaminaMp = stat.maxStaminaMP;
        _staminaMpRecoverySpeed = stat.staminaMpRecoverySpeed;
    }

    protected override void SetAttackWeight()
    {
        Dictionary<int, Data.WarriorAttackWeight> dict = Managers.Data.WarriorAttackWeightDict;
        Data.WarriorAttackWeight weight = dict[0];

        Define.AttackWeight basicComboOne = new Define.AttackWeight(_weapon, weight.basicComboOne);
        AttackWeight.Add("BasicComboOne", basicComboOne);

        Define.AttackWeight basicComboTwo = new Define.AttackWeight(_weapon, weight.basicComboTwo);
        AttackWeight.Add("BasicComboTwo", basicComboTwo);

        Define.AttackWeight basicComboThree = new Define.AttackWeight(_weapon, weight.basicComboThree);
        AttackWeight.Add("BasicComboThree", basicComboThree);

        Define.AttackWeight skillE = new Define.AttackWeight(null, weight.skillE);
        AttackWeight.Add("Warrior@SkillE", skillE);

        Define.AttackWeight skillR = new Define.AttackWeight(null, weight.skillR);
        AttackWeight.Add("Warrior@SkillR", skillR);

        Debug.Log(AttackWeight["BasicComboOne"].Weight);
    }

}
