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

        _effects["SkillE"].transform.parent = null;
        _effects["SkillR"].transform.parent = null;
        _uiStat = Managers.UI.ShowUI<UI_Stat>();
        _uiStat.PlayerStat = _playerStat;
    }

    protected override void RecoverMpStamina()
    {
        if (_playerStat.StaminaMp <= _playerStat.MaxStaminaMp && !_animator.GetBool("IsDodging") && !_animator.GetBool("IsAttacking"))
        {
            _playerStat.StaminaMp += _playerStat.StaminaMpRecoverySpeed * Time.deltaTime;

            if (_playerStat.StaminaMp > _playerStat.MaxStaminaMp)
                _playerStat.StaminaMp = _playerStat.MaxStaminaMp;
        }
    }

    protected override void OnDodgeEvent() 
    {
        ResetClickTriggers();

        transform.rotation = Quaternion.LookRotation(_movementDir);

        MoveForward((int)(_dashLength * 1000) + (int)(_dashDuration * 10));
    }
}
