using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MonsterStat : MonoBehaviour
{
    public MonsterStat MonsterStat { get; set; }

    [SerializeField]
    TMPro.TMP_Text _nameText;
    [SerializeField]
    Slider _hpBar;


    void Update()
    {
        SetHPBar();
    }

    public void SetName(string name)
    {
        _nameText.text = name;
    }

    public void SetHPBar()
    {
        float ratio = MonsterStat.Hp / (float)MonsterStat.MaxHp;
        _hpBar.value = ratio;
    }
}
