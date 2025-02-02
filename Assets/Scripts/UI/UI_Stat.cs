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
    [SerializeField]
    Image _lockOnIcon;

    [SerializeField]
    Stat _stat;
    Canvas _canvas;
    Transform _lockOnPoint;

    private void Start()
    {
        switch (Managers.Game.PlayerCalss)
        {
            case Define.PlayerClass.Wizard:
                _stat = Managers.Game.Player.GetComponent<WizardStat>();
                break;
            case Define.PlayerClass.Warrior:
                _stat = Managers.Game.Player.GetComponent<WarriorStat>();
                break;
        }
        
        _canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        SetHPBar();
        SetAbilityBar();
        SetLockOnIcon();
    }

    public void SetLockOnIcon()
    {
        if (_stat.Target != null)
        {
            if (!_lockOnIcon.enabled)
            {
                _lockOnIcon.enabled = true;

                _lockOnPoint = Util.FindDeepChild(_stat.Target, "LockOnPoint");
            }

            Vector3 screenPos = Camera.main.WorldToScreenPoint(_lockOnPoint.position);

            Vector2 uiPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.GetComponent<RectTransform>(), screenPos, _canvas.worldCamera, out uiPosition);

            _lockOnIcon.rectTransform.localPosition = uiPosition;
        }
        else if (_stat.Target == null && _lockOnIcon.enabled)
        {
            _lockOnIcon.enabled = false;
        }
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
