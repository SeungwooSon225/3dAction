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
    public Dictionary<int, Data.WarriorStat> WarriorStatDict { get; private set; } = new Dictionary<int, Data.WarriorStat>();
    public Dictionary<int, Data.WarriorStaminaMPConsumption> WarriorStaminaMPConsumptionDict { get; private set; } = new Dictionary<int, Data.WarriorStaminaMPConsumption>();
    public Dictionary<int, Data.WarriorAttackWeight> WarriorAttackWeightDict { get; private set; } = new Dictionary<int, Data.WarriorAttackWeight>();


    public void Init()
    {
        WarriorStatDict = LoadJson<Data.WarriorStatData, int, Data.WarriorStat>("WarriorStatData").MakeDict();
        WarriorStaminaMPConsumptionDict = LoadJson<Data.WarriorStaminaMPConsumptionData, int, Data.WarriorStaminaMPConsumption>("WarriorStaminaMPConsumptionData").MakeDict();
        WarriorAttackWeightDict = LoadJson<Data.WarriorAttackWeightData, int, Data.WarriorAttackWeight>("WarriorAttackWeightData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        Debug.Log(textAsset);
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
