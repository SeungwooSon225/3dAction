using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Attack
{
    [SerializeField]
    bool _isHeavy;

    [SerializeField]
    Vector3 _startOffset;
    [SerializeField]
    float _duration;
    [SerializeField]
    float _lifeTime;
    [SerializeField]
    float _distance;
    [SerializeField]
    ParticleSystem _effect;
    [SerializeField]
    ParticleSystem _explosionEffect;


    public void Shoot(Stat stat)
    {
        Damage = stat.Attack * stat.AttackWeight[gameObject.name].Weight;
        Debug.Log(stat.AttackWeight[gameObject.name].Weight + " " + gameObject.name);
        gameObject.GetComponent<Collider>().enabled = true;
        StartCoroutine(ShootCo(stat.gameObject.transform, stat.Target));
    }

    public void HalfParabolicShoot(Stat stat, Vector3 dir)
    {
        Attack attack = gameObject.GetComponent<Attack>();

        attack.Damage = stat.Attack * stat.AttackWeight[gameObject.name].Weight;

        gameObject.GetComponent<Collider>().enabled = true;

        StartCoroutine(HalfParabolicShootCo(stat.gameObject.transform, stat.Target, dir));
    }

    private IEnumerator HalfParabolicShootCo(Transform shooter, Transform target, Vector3 dir)
    {
        Vector3 startPosition = shooter.position +
            shooter.right * _startOffset.x +
            Vector3.up * _startOffset.y +
            shooter.forward * _startOffset.z;

        transform.position = startPosition;
        transform.rotation = Quaternion.LookRotation(shooter.forward);

        float elapsedTime = 0f;
        float speed = _distance / _duration;
        float elapsedDistance = 0f;

        dir = (transform.right * dir.x + transform.up * dir.y).normalized;

        // 포물선 움직임

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;

            elapsedDistance += 1f * Time.deltaTime;

            float height = 2f * Mathf.Sin(elapsedTime * 90f * Mathf.PI / 180f);
            transform.position = startPosition  + transform.forward * elapsedDistance + dir * height;

            yield return null;
        }

        transform.rotation = Quaternion.LookRotation(target.position + Vector3.up * 1f - transform.position);

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;

            transform.position += transform.forward * speed * Time.deltaTime;
            yield return null;
        }

        Managers.Resource.Destroy(gameObject);

        yield return null;
    }


    private IEnumerator ShootCo(Transform shooter, Transform target = null)
    {
        if (_effect != null)
            _effect.Stop();

        Vector3 startPosition = shooter.position;
        if (_startOffset.magnitude > 0.1f)
        {
            startPosition +=
            shooter.right * _startOffset.x +
            Vector3.up * _startOffset.y +
            shooter.forward * _startOffset.z;           
        }

        transform.position = startPosition;
        transform.rotation = Quaternion.LookRotation(shooter.forward);

        if (_effect != null)
            _effect.Play();

        float speed = _distance / _duration;
        float elapsedTime = 0f;

        if (_distance < 0.001f)
            yield return new WaitForSeconds(_duration);
        else
        {
            while (elapsedTime < _duration)
            {
                elapsedTime += Time.deltaTime;

                if (target != null)
                    transform.rotation = Quaternion.LookRotation(target.position + Vector3.up * 1.3f - transform.position);

                // 위치 업데이트
                //transform.position = Vector3.Slerp(startPosition, targetPosition, t);
                transform.position += transform.forward * speed * Time.deltaTime;

                if (target != null && (transform.position - target.position).magnitude < 2.0f)
                {
                    while (elapsedTime < _duration)
                    {
                        elapsedTime += Time.deltaTime;

                        transform.position += transform.forward * speed * Time.deltaTime;
                        yield return null;
                    }
                }

                yield return null;
            }
        }

        if (_effect != null)
            _effect.Stop();

        gameObject.GetComponent<Collider>().enabled = false;

        StartCoroutine(DestroyCo());
    }

    IEnumerator DestroyCo()
    {
        yield return new WaitForSeconds(_lifeTime);

        if (_explosionEffect != null)
        {
            _explosionEffect.transform.localPosition = Vector3.up * 100f;
        }

        Managers.Resource.Destroy(gameObject);
    }

    protected override void AttackOnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && IsPlayer) return;
        if (other.tag == "Monster" && !IsPlayer) return;

        if (!_isHeavy)
        {
            StopAllCoroutines();

            _effect.Stop();
            gameObject.GetComponent<Collider>().enabled = false;

            if (_explosionEffect != null)
            {
                //_explosionEffect.Stop();
                _explosionEffect.transform.localPosition = Vector3.zero;
                _explosionEffect.Play();
            }

            StartCoroutine(DestroyCo());

            if(other.tag == "Obstacle")
                return;
        }


        //Debug.Log(other.name);
        Stat stat = other.GetComponent<Stat>();

        if (stat == null) return;

        stat.OnAttacked(this);
    }
}
