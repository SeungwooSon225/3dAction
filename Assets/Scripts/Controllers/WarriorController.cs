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

    void Update()
    {
        Moving();
        RecoverStamina();
        Skill();
    }

    protected override void Init()
    {
        base.Init();

        _warriorStat = _playerStat as WarriorStat;
        //_warriorStat.
        //_attackRatio.Add("BasicComboOne", 1.0f);
        //_attackRatio.Add("BasicComboTwo", 1.2f);

        //Define.AttackWeight basicComboOne = new Define.AttackWeight(_attack, 1.0f);
        //_playerStat.AttackWeight.Add("BasicComboOne", basicComboOne);
        //Define.AttackWeight basicComboTwo = new Define.AttackWeight(_attack, 1.2f);
        //_playerStat.AttackWeight.Add("BasicComboTwo", basicComboTwo);
        //Define.AttackWeight skillE = new Define.AttackWeight(null, 2.0f);
        //_playerStat.AttackWeight.Add("Warrior@SkillE", skillE);
        //Define.AttackWeight skillR = new Define.AttackWeight(null, 3.0f);
        //_playerStat.AttackWeight.Add("Warrior@SkillR", skillR);

        //_playerStat.StaminaMpConsumption.Add("BasicAttack", 10f);
        //_playerStat.StaminaMpConsumption.Add("SkillE", 0f);
        //_playerStat.StaminaMpConsumption.Add("SkillR", 0f);
    }

    private void RecoverStamina()
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
