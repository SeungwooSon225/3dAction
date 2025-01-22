using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorStat : PlayerStat
{
    [SerializeField]
    Attack _weapon;

    void Start()
    {
        Init();
    }

    protected override void Init()
    {
        _playerClass = Define.PlayerClass.Warrior;

        base.Init();

        SetAttackWeight();
    }

    //protected override void SetStat(int level)
    //{
    //    Dictionary<int, Data.WarriorStat> dict = Managers.Data.WarriorStatDict;
    //    Data.WarriorStat stat = dict[level];

    //    _hp = stat.maxHp;
    //    _maxHp = stat.maxHp;
    //    _attack = stat.attack;
    //    _defense = stat.defense;
    //    _moveSpeed = stat.moveSpeed;
    //    _staminaMp = stat.maxStaminaMP;
    //    _maxStaminaMp = stat.maxStaminaMP;
    //    _staminaMpRecoverySpeed = stat.staminaMpRecoverySpeed;
    //}

    protected override void SetAttackWeight()
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        Data.Stat stat = dict[1];

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

}
