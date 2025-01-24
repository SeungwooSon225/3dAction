using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardSkillE : MonoBehaviour
{
    [SerializeField]
    Vector3 _startOffset;

    [SerializeField]
    float _duration;

    [SerializeField]
    ParticleSystem _effect;

    [SerializeField]
    Attack _attack;

    [SerializeField]
    Transform _laser;
    [SerializeField]
    Transform _trailEffect;

    public void Shoot(Stat stat)
    {
        Transform shooter = stat.transform;
        _attack.Damage = stat.Attack * stat.AttackWeight[gameObject.name].Weight;

        //attack.IsActive = true;
        gameObject.GetComponent<Collider>().enabled = true;

        if (_effect != null)
            _effect.Stop();

        if ((_startOffset - Vector3.zero).magnitude > 0.1f)
        {
            Vector3 startPosition = shooter.position +
            shooter.right * _startOffset.x +
            Vector3.up * _startOffset.y +
            shooter.forward * _startOffset.z;
            transform.position = startPosition;
        }

        transform.rotation = Quaternion.LookRotation(shooter.forward);

        gameObject.SetActive(true);
        if (_effect != null)
            _effect.Play();
        
        StartCoroutine(ShootCo());
    }

    IEnumerator ShootCo()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);

        float elapsedTime = 0f;
        float scale = 0f;

        _trailEffect.localPosition = Vector3.zero;

        while (elapsedTime < 0.25f)
        {
            elapsedTime += Time.deltaTime;
            scale = elapsedTime / 0.25f;

            _trailEffect.localPosition = new Vector3(0f, 0f, 12f * scale);

            yield return null;
        }

        yield return new WaitForSeconds(0.15f);

        elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            scale = 1 - elapsedTime / _duration;

            _laser.localScale = new Vector3(6f * scale, 6f * scale, 15f);

            yield return null;
        }

        if (_effect != null)
            _effect.Stop();
        gameObject.SetActive(false);

        gameObject.GetComponent<Collider>().enabled = false;
    }
}
