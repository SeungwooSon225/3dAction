using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UI_StatusPopup : UI_Base
{
    enum StatType
    { 
        Hp,
        StaminaMp,
        Attack,
        Defense,
    }

    #region Buttons
    [SerializeField]
    TMPro.TMP_Text _levelText;
    [SerializeField]
    TMPro.TMP_Text _costText;

    [SerializeField]
    TMPro.TMP_Text _hpText;
    [SerializeField]
    GameObject _hpDecreaseButton;
    [SerializeField]
    GameObject _hpIncreaseButton;

    [SerializeField]
    TMPro.TMP_Text _staminaMpText;
    [SerializeField]
    GameObject _staminaMpDecreaseButton;
    [SerializeField]
    GameObject _staminaMpIncreaseButton;

    [SerializeField]
    TMPro.TMP_Text _attackText;
    [SerializeField]
    GameObject _attackDecreaseButton;
    [SerializeField]
    GameObject _attackIncreaseButton;

    [SerializeField]
    TMPro.TMP_Text _defenseText;
    [SerializeField]
    GameObject _defenseDecreaseButton;
    [SerializeField]
    GameObject _defenseIncreaseButton;
    #endregion


    int _cost = 0;
    int _hpChange = 5;
    int _staminaMpChange = 5;
    int _attackChange = 2;
    int _defenseChange = 1;

    public override void Init()
    {
        BindEvent(_hpDecreaseButton, OnHpDecreaseButtonClicked, Define.UIEvent.Click);
        BindEvent(_hpIncreaseButton, OnHpIncreaseButtonClicked, Define.UIEvent.Click);

        BindEvent(_staminaMpDecreaseButton, OnStaminaMpDecreaseButtonClicked, Define.UIEvent.Click);
        BindEvent(_staminaMpIncreaseButton, OnStaminaMpIncreaseButtonClicked, Define.UIEvent.Click);

        BindEvent(_attackDecreaseButton, OnAttackDecreaseButtonClicked, Define.UIEvent.Click);
        BindEvent(_attackIncreaseButton, OnAttackIncreaseButtonClicked, Define.UIEvent.Click);

        BindEvent(_defenseDecreaseButton, OnDefenseDecreaseButtonClicked, Define.UIEvent.Click);
        BindEvent(_defenseIncreaseButton, OnDefenseIncreaseButtonClicked, Define.UIEvent.Click);
    }

    int DecreaseLevel()
    {
        int currentLevel = int.Parse(_levelText.text);
        int newLevel = currentLevel - 1;

        if (newLevel < Managers.Game.PlayerStat.Level)
            return 0;

        return newLevel;
    }
    void UpdateLevel(int newLevel)
    {
        _levelText.text = newLevel.ToString();
        if (newLevel == Managers.Game.PlayerStat.Level)
        {
            _levelText.color = Color.white;
        }
        else
        {
            _levelText.color = Color.blue;
        }
    }

    void DecreaseCost()
    {
        _cost -= int.Parse(_levelText.text) * 100;
        _costText.text = _cost.ToString();
        if (_cost == 0)
            _costText.color = Color.white;
        else
            _costText.color = Color.blue;
    }

    bool IncreaseCost()
    {
        if (_cost > Managers.Game.PlayerStat.Gold)
            return false;

        int currentLevel = int.Parse(_levelText.text);
        int newLevel = currentLevel + 1;
        _levelText.text = newLevel.ToString();
        _levelText.color = Color.blue;

        _cost += currentLevel * 100;
        _costText.text = _cost.ToString();

        if (_cost > Managers.Game.PlayerStat.Gold)
            _costText.color = Color.red;
        else
            _costText.color = Color.blue;

        return true;
    }

    void DecreaseStat(StatType statType, TMPro.TMP_Text statText, int statChange)
    {
        int newLevel = DecreaseLevel();
        if (newLevel == 0) return;

        int currentStat = int.Parse(statText.text);
        int newStat = currentStat -= statChange;

        switch (statType)
        {
            case StatType.Hp:
                if (newStat < Managers.Game.PlayerStat.MaxHp)
                    return;
                break;

            case StatType.StaminaMp:
                if (newStat < Managers.Game.PlayerStat.MaxStaminaMp)
                    return;
                break;

            case StatType.Attack:
                if (newStat < Managers.Game.PlayerStat.Attack)
                    return;
                break;

            case StatType.Defense:
                if (newStat < Managers.Game.PlayerStat.Defense)
                    return;
                break;
        }

        UpdateLevel(newLevel);

        statText.text = newStat.ToString();

        int originalStat = 0;
        switch (statType)
        {
            case StatType.Hp:
                originalStat = (int)Managers.Game.PlayerStat.MaxHp;
                break;

            case StatType.StaminaMp:
                originalStat = (int)Managers.Game.PlayerStat.MaxStaminaMp;
                break;

            case StatType.Attack:
                originalStat = (int)Managers.Game.PlayerStat.Attack;
                break;

            case StatType.Defense:
                originalStat = (int)Managers.Game.PlayerStat.Defense;
                break;
        }

        if (newStat == originalStat)
        {
            statText.color = Color.white;
        }
        else
        {
            statText.color = Color.blue;
        }

        DecreaseCost();
    }

    void IncreaseStat(TMPro.TMP_Text statText, int statChange)
    {
        if (!IncreaseCost())
            return;

        int currentStat = int.Parse(statText.text);
        int newStat = currentStat += statChange;
        statText.text = newStat.ToString();
        statText.color = Color.blue;
    }


    void OnHpDecreaseButtonClicked(PointerEventData data)
    {
        DecreaseStat(StatType.Hp, _hpText, _hpChange);
        //int newLevel = DecreaseLevel();
        //if (newLevel == 0) return;

        //int currentHp = int.Parse(_hpText.text);
        //int newHp = currentHp -= _hpChange;

        //if (newHp < Managers.Game.PlayerStat.MaxHp)
        //    return;

        //UpdateLevel(newLevel);

        //_hpText.text = newHp.ToString();
        //if (newHp == Managers.Game.PlayerStat.MaxHp)
        //{
        //    _hpText.color = Color.white;
        //}
        //else
        //{
        //    _hpText.color = Color.blue;
        //}

        //DecreaseCost();
    }

    void OnHpIncreaseButtonClicked(PointerEventData data)
    {
        IncreaseStat(_hpText, _hpChange);
        //if (!IncreaseCost())
        //    return;

        //int currentHp = int.Parse(_hpText.text);
        //int newHp = currentHp += _hpChange;
        //_hpText.text = newHp.ToString();
        //_hpText.color = Color.blue; 
    }

    void OnStaminaMpDecreaseButtonClicked(PointerEventData data)
    {
        DecreaseStat(StatType.StaminaMp, _staminaMpText, _staminaMpChange);
        //int newLevel = DecreaseLevel();
        //if (newLevel == 0) return;

        //int currentStaminaMp = int.Parse(_staminaMpText.text);
        //int newStaminaMp = currentStaminaMp -= _staminaMpChange;

        //if (newStaminaMp < Managers.Game.PlayerStat.MaxStaminaMp)
        //    return;

        //UpdateLevel(newLevel);

        //_staminaMpText.text = newStaminaMp.ToString();
        //if (newStaminaMp == Managers.Game.PlayerStat.MaxStaminaMp)
        //{
        //    _staminaMpText.color = Color.white;
        //}
        //else
        //{
        //    _staminaMpText.color = Color.blue;
        //}

        //DecreaseCost();
    }

    void OnStaminaMpIncreaseButtonClicked(PointerEventData data)
    {
        IncreaseStat(_staminaMpText, _staminaMpChange);
        //if (!IncreaseCost())
        //    return;

        //int currentStaminaMp = int.Parse(_staminaMpText.text);
        //int newStaminaMp = currentStaminaMp += _staminaMpChange;
        //_staminaMpText.text = newStaminaMp.ToString();
        //_staminaMpText.color = Color.blue;
    }

    void OnAttackDecreaseButtonClicked(PointerEventData data)
    {
        DecreaseStat(StatType.Attack, _attackText, _attackChange);
    }

    void OnAttackIncreaseButtonClicked(PointerEventData data)
    {
        IncreaseStat(_attackText, _attackChange);
    }

    void OnDefenseDecreaseButtonClicked(PointerEventData data)
    {
        DecreaseStat(StatType.Defense, _defenseText, _defenseChange);
    }

    void OnDefenseIncreaseButtonClicked(PointerEventData data)
    {
        IncreaseStat(_defenseText, _defenseChange);
    }

}
