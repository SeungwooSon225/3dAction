using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorController : PlayerController
{
    float _dashLength = 3.5f;
    float _dashDuration = 0.3f;



    //[SerializeField]
    //Attack _attack;

    WarriorStat _warriorStat;

    enum WarriorAttack
    { 
        BasicComboOne = 10,
        BasicComboTwo = 13,
    }

    protected override void Init()
    {
        base.Init();

        _warriorStat = _playerStat as WarriorStat;

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

        StartCoroutine(MoveForwardCo(_dashLength, _dashDuration));
    }

    //private IEnumerator Dash()
    //{
    //    transform.rotation = Quaternion.LookRotation(_movementDir);

    //    Vector3 startPosition = transform.position;
    //    Vector3 targetPosition = startPosition + transform.forward * _dashLength;
    //    float elapsedTime = 0f;

    //    while (elapsedTime < _dashDuration)
    //    {
    //        elapsedTime += Time.deltaTime;

    //        float t = elapsedTime / _dashDuration; // 진행 비율 (0~1)

    //        // 위치 업데이트
    //        if (!Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward.normalized, out RaycastHit hit, 0.5f) ||
    //            !hit.collider.CompareTag("Obstacle"))
    //        {
    //            transform.position = Vector3.Slerp(startPosition, targetPosition, t);
    //        }

    //        yield return null;
    //    }
    //}

    #region Animation
    //private void SetAttackActive(int value)
    //{
    //    _attack.GetComponent<Collider>().enabled = (value == 0 ? false : true);
    //    //_attack.IsActive = value == 0 ? false : true;
    //}

    //private void SetAttackDamage(string attackName)
    //{
    //    SetDamage(_attack, attackName);
    //}

    #endregion Animation
}
