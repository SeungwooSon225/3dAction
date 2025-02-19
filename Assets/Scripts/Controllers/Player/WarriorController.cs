using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorController : PlayerController
{
    float _dashLength = 3.5f;
    float _dashDuration = 0.3f;

    protected override void Init()
    {
        base.Init();

        Transform skillE = Util.FindDeepChild(transform, "SkillE");
        if (skillE != null)
        {
            _effects.Add("SkillE", skillE.GetComponent<ParticleSystem>());
            skillE.transform.parent = null;
        }
        Transform skillR = Util.FindDeepChild(transform, "SkillR");
        if (skillR != null)
        {
            _effects.Add("SkillR", skillR.GetComponent<ParticleSystem>());
        }

        _uiStat = Managers.UI.ShowUI<UI_Stat>();
        _uiStat.PlayerStat = _playerStat;
    }

    protected override void RecoverMpStamina()
    {
        if (_playerStat.StaminaMp <= _playerStat.MaxStaminaMp && !IsDodging && !IsAttacking)
        {
            _playerStat.StaminaMp += _playerStat.StaminaMpRecoverySpeed * Time.deltaTime;

            if (_playerStat.StaminaMp > _playerStat.MaxStaminaMp)
                _playerStat.StaminaMp = _playerStat.MaxStaminaMp;
        }
    }

    protected override void OnDodgeEvent() 
    {
        ResetClickTriggers();

        transform.rotation = Quaternion.LookRotation(MovementDir);

        MoveForward((int)(_dashLength * 1000) + (int)(_dashDuration * 10));
    }
}
