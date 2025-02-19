using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Stat : MonoBehaviour
{
    public PlayerStat PlayerStat { get; set; }
    bool _isSkillECool;
    bool _isSkillRCool;
    public bool IsSkillECool 
    {
        get { return _isSkillECool; }
        set 
        {
            _isSkillECool = value;
            if (_isSkillECool == true)
            {
                StartCoroutine(SkillECoolDownCo());
            }
        }
    }
    public bool IsSkillRCool
    {
        get { return _isSkillRCool; }
        set
        {
            _isSkillRCool = value;
            if (_isSkillRCool == true)
            {
                StartCoroutine(SkillRCoolDownCo());
            }
        }
    }

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
    TMPro.TMP_Text _goldText;

    
    Canvas _canvas;
    Transform _lockOnPoint;

    private void Start()
    {
        switch (Managers.Game.PlayerClass)
        {
            case Define.PlayerClass.Wizard:
                PlayerStat = Managers.Game.Player.GetComponent<WizardStat>();
                break;
            case Define.PlayerClass.Warrior:
                PlayerStat = Managers.Game.Player.GetComponent<WarriorStat>();
                break;
        }
        
        _canvas = GetComponent<Canvas>();
        _goldText.text = PlayerStat.Gold.ToString();
    }

    void Update()
    {        
        SetHPBar();
        SetAbilityBar();
        SetLockOnIcon();
    }

    public void SetLockOnIcon()
    {
        if (PlayerStat.Target != null)
        {
            if (!_lockOnIcon.enabled)
            {
                _lockOnIcon.enabled = true;

                _lockOnPoint = Util.FindDeepChild(PlayerStat.Target, "LockOnPoint");
            }

            Vector3 screenPos = Camera.main.WorldToScreenPoint(_lockOnPoint.position);

            Vector2 uiPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.GetComponent<RectTransform>(), screenPos, _canvas.worldCamera, out uiPosition);

            _lockOnIcon.rectTransform.localPosition = uiPosition;
        }
        else if (PlayerStat.Target == null && _lockOnIcon.enabled)
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

    public IEnumerator SkillECoolDownCo()
    {
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

    public IEnumerator SkillRCoolDownCo()
    {
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

    public void UpdateGold()
    {
        //_goldText.text = PlayerStat.Gold.ToString();

        StartCoroutine(UpdateGoldCo());
    }

    IEnumerator UpdateGoldCo()
    {
        int currentGold = int.Parse(_goldText.text);
        int newGold = PlayerStat.Gold;

        int addGold = (newGold - currentGold) / 100;

        do
        {
            currentGold += addGold;
            _goldText.text = currentGold.ToString();
            yield return new WaitForSeconds(0.01f);
        }
        while (currentGold < newGold);

        currentGold = newGold;
        _goldText.text = currentGold.ToString();
    }
}
