using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardSkillController : MonoBehaviour
{
    [SerializeField]
    WizardMaterialController _wizardMaterialController;

    WizardController _wizardController;
    Collider _wizardSkillRCollider;

    Transform _staff;
    GameObject _chargeAttack;

    IEnumerator _chargeCo;

    protected Dictionary<string, ParticleSystem> _effects = new Dictionary<string, ParticleSystem>();

    private void Start()
    {
        Transform skillR = Util.FindDeepChild(transform, "SkillR");
        if (skillR != null)
        {
            _effects.Add("SkillR", skillR.GetComponent<ParticleSystem>());
            _wizardSkillRCollider = skillR.GetComponent<Collider>();
        }
    }

    public void StopCharge()
    {
        if (_chargeCo != null)
        {
            StopCoroutine(_chargeCo);
            _chargeAttack.GetComponent<ParticleSystem>().Stop();
            Managers.Resource.Destroy(_chargeAttack);
        }
    }

    void ChargeAttackStart()
    {
        _chargeAttack = Managers.Resource.Instantiate($"Projectiles/Wizard@ChargeAttack");

        _chargeAttack.GetComponent<ParticleSystem>().Stop();
        _chargeAttack.transform.parent = _staff;
        _chargeAttack.transform.localPosition = Vector3.zero;
        _chargeAttack.GetComponent<ParticleSystem>().Play();

        if (_chargeCo != null) StopCoroutine(_chargeCo);
        _chargeCo = ChargeCo();
        StartCoroutine(_chargeCo);
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
        if (_chargeCo != null) StopCoroutine(_chargeCo);
        _chargeAttack.transform.parent = null;
        _chargeAttack.GetComponent<Projectile>().Shoot(_wizardController.PlayerStat);
    }

    private void SkillE()
    {
        GameObject skillE = Managers.Resource.Instantiate($"Projectiles/Wizard@SkillE");
        skillE.GetComponent<WizardSkillE>().Shoot(_wizardController.PlayerStat);
    }

    private void SkillR()
    {
        _wizardSkillRCollider.GetComponent<Attack>().Damage = _wizardController.PlayerStat.Attack * _wizardController.PlayerStat.AttackWeight["Wizard@SkillR"].Weight;
        _effects["SkillR"].Play();
    }

    IEnumerator SkillRExplosionCo()
    {
        _wizardSkillRCollider.enabled = true;

        yield return new WaitForSeconds(0.2f);

        _wizardSkillRCollider.enabled = false;
    }

    private void TeleportStart()
    {
        _wizardMaterialController.FadeOut();
    }

    private void TeleportEnd()
    {
        _wizardMaterialController.FadeIn();
    }
}
