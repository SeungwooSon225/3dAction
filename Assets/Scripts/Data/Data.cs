using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Data
{
    #region Stat
    [Serializable]
    public class WarriorStat
    {
        public int level;
        public float maxHp;
        public float attack;
        public float defense;
        public float moveSpeed;
        public float maxStaminaMP;
        public float staminaMpRecoverySpeed;
        public float totalExp;
    }

    [Serializable]
    public class WarriorStatData : ILoader<int, WarriorStat>
    {
        public List<WarriorStat> warriorStats = new List<WarriorStat>();

        public Dictionary<int, WarriorStat> MakeDict()
        {
            Dictionary<int, WarriorStat> dict = new Dictionary<int, WarriorStat>();
            foreach (WarriorStat stat in warriorStats)
                dict.Add(stat.level, stat);
            return dict;
        }
    }
    #endregion

    #region StaminaMPConsumption
    [Serializable]
    public class WarriorStaminaMPConsumption
    {
        public int level;
        public float dodge;
        public float basicAttack;
        public float skillE;
        public float skillR;
    }

    [Serializable]
    public class WarriorStaminaMPConsumptionData : ILoader<int, WarriorStaminaMPConsumption>
    {
        public List<WarriorStaminaMPConsumption> warriorStaminaMPConsumptions = new List<WarriorStaminaMPConsumption>();

        public Dictionary<int, WarriorStaminaMPConsumption> MakeDict()
        {
            Dictionary<int, WarriorStaminaMPConsumption> dict = new Dictionary<int, WarriorStaminaMPConsumption>();
            foreach (WarriorStaminaMPConsumption consumption in warriorStaminaMPConsumptions)
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
