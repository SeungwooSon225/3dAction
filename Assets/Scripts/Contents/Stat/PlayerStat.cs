using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    protected Dictionary<string, float> _staminaMpConsumption = new Dictionary<string, float>();

    [SerializeField]
    protected float _staminaMp;
    [SerializeField]
    protected float _maxStaminaMp;
    [SerializeField]
    protected float _staminaMpRecoverySpeed;

    protected int _exp;
    protected int _gold;

    public Dictionary<string, float> StaminaMpConsumption { get { return _staminaMpConsumption; } }
    public float StaminaMp { get { return _staminaMp; } set { _staminaMp = value; } }
    public float MaxStaminaMp { get { return _maxStaminaMp; } set { _maxStaminaMp = value; } }
    public float StaminaMpRecoverySpeed { get { return _staminaMpRecoverySpeed; } set { _staminaMpRecoverySpeed = value; } }

    public int Exp { get { return _exp; } set { _hp = value; } }
    public int Gold { get { return _gold; } set { _hp = value; } }

    protected virtual void SetStat(int level) { }

    public void SetStaminaMpConsumption(int level)
    {
        if (gameObject.GetComponent<WarriorStat>() != null)
        {
            Dictionary<int, Data.WarriorStaminaMPConsumption> dict = Managers.Data.WarriorStaminaMPConsumptionDict;
            Data.WarriorStaminaMPConsumption consumption = dict[level];

            _staminaMpConsumption.Add("Dodge", consumption.dodge);
            _staminaMpConsumption.Add("BasicAttack", consumption.basicAttack);
            _staminaMpConsumption.Add("SkillE", consumption.skillE);
            _staminaMpConsumption.Add("SkillR", consumption.skillR);

        }
    }
}
