using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : PlayerController
{
    [SerializeField]
    Transform _staff;

    GameObject _chargeAttack;
    Projectile _chargeAttackProjectile;

    Collider wizardSkillR;

    [SerializeField]
    WizardMaterialController _wizardMaterialController;

    void Update()
    {
        Moving();
        Skill();
        RecoverMana();
    }

    protected override void Init()
    {
        base.Init();

        Transform skillR = Util.FindDeepChild(transform, "SkillR");
        if (skillR != null)
        {
            _effects.Add("SkillR", skillR.GetComponent<ParticleSystem>());
            wizardSkillR = skillR.GetComponent<Collider>();

            // Todo
            skillR.GetComponent<Attack>().Damage = 20f;
        }

        UI_Stat uiStat = Managers.UI.ShowUI<UI_Stat>("UI_WizardStat");
        uiStat.PlayerStat = _playerStat;
    }

    Vector3 _previousPos = Vector3.zero;
    float _stopTime;
    [SerializeField]
    bool _isStop = false;


    private void RecoverMana()
    {
        if (!_isStop && (transform.position - _previousPos).magnitude < 0.01f)
        {
            _isStop = true;
            _stopTime = Time.time;
        }
        else if (_isStop && (transform.position - _previousPos).magnitude >= 0.01f)
        {
            _isStop = false;
            _stopTime = 0f;
        }

        _previousPos = transform.position;

        if (_playerStat.StaminaMp < _playerStat.MaxStaminaMp && !_animator.GetBool("IsDodging") && !_animator.GetBool("IsAttacking"))
        {
            if(_isStop && (Time.time - _stopTime) > 1f)
                _playerStat.StaminaMp += _playerStat.StaminaMpRecoverySpeed * Time.deltaTime * 5f;
            else
                _playerStat.StaminaMp += _playerStat.StaminaMpRecoverySpeed * Time.deltaTime;

            if (_playerStat.StaminaMp > _playerStat.MaxStaminaMp)
                _playerStat.StaminaMp = _playerStat.MaxStaminaMp;
        }     
    }

    void ChargeAttackStart()
    {
        _chargeAttack = Managers.Resource.Instantiate($"Projectiles/Wizard@ChargeAttack");

        _chargeAttackProjectile = _chargeAttack.GetComponent<Projectile>();
        _chargeAttack.GetComponent<ParticleSystem>().Stop();
        _chargeAttack.transform.parent = _staff;
        _chargeAttack.transform.localPosition = Vector3.zero;
        _chargeAttack.GetComponent<ParticleSystem>().Play();

        StartCoroutine(ChargeCo());
    }

    IEnumerator ChargeCo()
    {
        _chargeAttack.transform.localScale = Vector3.one;

        float _elapsedTime = 0f;
        while (_elapsedTime < 2f)
        {
            _elapsedTime += Time.deltaTime;

            _chargeAttack.transform.localScale = Vector3.one + Vector3.one * _elapsedTime / 1f;

            yield return null;
        }
    }

    void ShootChargeAttack()
    {
        _chargeAttack.transform.parent = null;
        _chargeAttackProjectile.Shoot(_playerStat);
    }

    private void SkillE()
    {
        GameObject skillE = Managers.Resource.Instantiate($"Projectiles/Wizard@SkillE");
        skillE.GetComponent<WizardSkillE>().Shoot(_playerStat);
    }

    private void SkillR()
    {
        _effects["SkillR"].Play();
    }

    IEnumerator SkillRExplosionCo()
    {
        wizardSkillR.enabled = true;

        yield return new WaitForSeconds(0.5f);

        wizardSkillR.enabled = false;
    }

    private void TeleportStart()
    {
        StartCoroutine(_wizardMaterialController.FadeOutCo());
    }

    private void TeleportEnd()
    {
        StartCoroutine(_wizardMaterialController.FadeInCo());
    }
}
