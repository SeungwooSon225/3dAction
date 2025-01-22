using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Data
{
    #region Stat
    [Serializable]
    public class Stat
    {
        public int level;
        public float maxHp;
        public float attack;
        public float defense;
        public float moveSpeed;
        public float maxStaminaMP;
        public float staminaMpRecoverySpeed;
        public float totalExp;

        public float dodgeConsumption;
        public float basicAttackConsumption;
        public float skillEConsumption;
        public float skillRConsumption;

        public float basicComboOneWeight;
        public float basicComboTwoWeight;
        public float basicComboThreeWeight;
        public float skillEWeight;
        public float skillRWeight;
    }

    [Serializable]
    public class StatData : ILoader<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();

        public Dictionary<int, Stat> MakeDict()
        {
            Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
            foreach (Stat stat in stats)
                dict.Add(stat.level, stat);
            return dict;
        }
    }
    #endregion

    #region StaminaMPConsumption
    [Serializable]
    public class StaminaMPConsumption
    {
        public int level;
        public float dodge;
        public float basicAttack;
        public float skillE;
        public float skillR;
    }

    [Serializable]
    public class StaminaMPConsumptionData : ILoader<int, StaminaMPConsumption>
    {
        public List<StaminaMPConsumption> staminaMPConsumptions = new List<StaminaMPConsumption>();

        public Dictionary<int, StaminaMPConsumption> MakeDict()
        {
            Dictionary<int, StaminaMPConsumption> dict = new Dictionary<int, StaminaMPConsumption>();
            foreach (StaminaMPConsumption consumption in staminaMPConsumptions)
                dict.Add(consumption.level, consumption);
            return dict;
        }
    }
    #endregion

    #region AttackWeight
    [Serializable]
    public class WarriorAttackWeight
    {
        public int level;
        public float basicComboOne;
        public float basicComboTwo;
        public float basicComboThree;
        public float skillE;
        public float skillR;
    }

    [Serializable]
    public class WarriorAttackWeightData : ILoader<int, WarriorAttackWeight>
    {
        public List<WarriorAttackWeight> warriorAttackWeight = new List<WarriorAttackWeight>();

        public Dictionary<int, WarriorAttackWeight> MakeDict()
        {
            Dictionary<int, WarriorAttackWeight> dict = new Dictionary<int, WarriorAttackWeight>();
            foreach (WarriorAttackWeight weight in warriorAttackWeight)
                dict.Add(weight.level, weight);
            return dict;
        }
    }
    #endregion
}
