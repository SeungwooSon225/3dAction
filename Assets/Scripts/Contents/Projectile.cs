using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    Vector3 startOffset;
    [SerializeField]
    float _duration;
    [SerializeField]
    float _distance;
    [SerializeField]
    float _lifeTime;


    public void Shoot(Stat stat)
    {
        Attack attack = gameObject.GetComponent<Attack>();

        attack.Damage = stat.Attack *  stat.AttackWeight[gameObject.name].Weight;

        //attack.IsActive = true;
        gameObject.GetComponent<Collider>().enabled = true;

        //StartCoroutine(ShootCo(stat.gameObject.transform, stat.Target));
        StartCoroutine(HalfParabolicShootCo(stat.gameObject.transform, stat.Target));

    }


    private IEnumerator HalfParabolicShootCo(Transform shooter, Transform target)
    {
        Vector3 startPosition = shooter.position +
            shooter.right * startOffset.x +
            Vector3.up * startOffset.y +
            shooter.forward * startOffset.z;

        transform.position = startPosition;
        transform.rotation = Quaternion.LookRotation(shooter.forward);

        float elapsedTime = 0f;
        float speed = _distance / _duration;
        float elapsedDistance = 0f;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;

            elapsedDistance += speed * Time.deltaTime;

            float height = 1f * Mathf.Sin(elapsedTime * 90f * Mathf.PI / 180f);
            transform.position = startPosition  + transform.forward * elapsedDistance + Vector3.up * height;

            yield return null;
        }

        Managers.Resource.Destroy(gameObject);

        yield return null;
    }


    private IEnumerator ShootCo(Transform shooter, Transform target = null)
    {
        Vector3 startPosition = shooter.position +
            shooter.right * startOffset.x +
            Vector3.up * startOffset.y +
            shooter.forward * startOffset.z;

        transform.position = startPosition;
        transform.rotation = Quaternion.LookRotation(shooter.forward);

        float speed = _distance / _duration;
        float elapsedTime = 0f;
        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;

            if (target != null)
                transform.rotation = Quaternion.LookRotation(target.position + Vector3.up * 1.3f - transform.position);

            // 위치 업데이트
            //transform.position = Vector3.Slerp(startPosition, targetPosition, t);
            transform.position += transform.forward * speed * Time.deltaTime;

            if ((transform.position - target.position).magnitude < 2.0f)
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

        yield return new WaitForSeconds(_lifeTime);

        gameObject.GetComponent<Collider>().enabled = false;

        Managers.Resource.Destroy(gameObject);
    }
}
