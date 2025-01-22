using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.Stat> StatDict { get; private set; } = new Dictionary<int, Data.Stat>();
    //public Dictionary<int, Data.StaminaMPConsumption> StaminaMPConsumptionDict { get; private set; } = new Dictionary<int, Data.StaminaMPConsumption>();
    //public Dictionary<int, Data.WarriorAttackWeight> AttackWeightDict { get; private set; } = new Dictionary<int, Data.WarriorAttackWeight>();


    public void Init()
    {
        //InitPlayerStat(Define.PlayerClass.Warrior);
        //Debug.Log(Define.PlayerClass.Warrior);
        //WarriorStatDict = LoadJson<Data.WarriorStatData, int, Data.WarriorStat>("WarriorStatData").MakeDict();
        //WarriorStaminaMPConsumptionDict = LoadJson<Data.WarriorStaminaMPConsumptionData, int, Data.WarriorStaminaMPConsumption>("WarriorStaminaMPConsumptionData").MakeDict();
        //WarriorAttackWeightDict = LoadJson<Data.WarriorAttackWeightData, int, Data.WarriorAttackWeight>("WarriorAttackWeightData").MakeDict();
    }

    public void InitPlayerStat(Define.PlayerClass playerClass)
    {
        StatDict = LoadJson<Data.StatData, int, Data.Stat>($"{playerClass.ToString()}StatData").MakeDict();
        //StaminaMPConsumptionDict = LoadJson<Data.StaminaMPConsumptionData, int, Data.StaminaMPConsumption>($"{playerClass.ToString()}StaminaMPConsumptionData").MakeDict();
        //AttackWeightDict = LoadJson<Data.WarriorAttackWeightData, int, Data.WarriorAttackWeight>($"{playerClass.ToString()}AttackWeightData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        //Debug.Log(textAsset);
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
