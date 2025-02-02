using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Stat : MonoBehaviour
{
    public PlayerStat PlayerStat { get; set; }
    public bool IsSkillECool { get; set; } = false;
    public bool IsSkillRCool { get; set; } = false;

    [SerializeField]
    Slider _hpBar;
    [SerializeField]
    Slider _staminaMpBar;

    [SerializeField]
    Image _skillEIcon;
    [SerializeField]
    Image _skillRIcon;


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

    public IEnumerator SkillECoolDown()
    {
        IsSkillECool = true;
        _skillEIcon.fillAmount = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < 10f)
        {
            elapsedTime += Time.deltaTime;

            _skillEIcon.fillAmount = elapsedTime / 10f;

            yield return null;
        }

        IsSkillECool = false;
        _skillEIcon.fillAmount = 1f;
    }

    public IEnumerator SkillRCoolDown()
    {
        IsSkillRCool = true;
        _skillRIcon.fillAmount = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < 10f)
        {
            elapsedTime += Time.deltaTime;

            _skillRIcon.fillAmount = elapsedTime / 10f;

            yield return null;
        }

        IsSkillRCool = false;
        _skillRIcon.fillAmount = 1f;
    }
}
