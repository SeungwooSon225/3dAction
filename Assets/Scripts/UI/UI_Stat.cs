using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Stat : MonoBehaviour
{
    public PlayerStat PlayerStat { get; set; }

    [SerializeField]
    Slider _hpBar;
    [SerializeField]
    Slider _staminaMpBar;


    void Update()
    {
        SetHPBar();
        SetAbilityBar();
    }

    public void SetHPBar()
    {
        float ratio = PlayerStat.Hp / (float)PlayerStat.MaxHp;
        _hpBar.value = ratio;
    }

    public void SetAbilityBar()
    {
        float ratio = PlayerStat.StaminaMp / (float)PlayerStat.MaxStaminaMp;
        _staminaMpBar.value = ratio;
    }
}
