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

        StartCoroutine(ShootCo());
    }

    protected IEnumerator ShootCo()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        transform.rotation = Quaternion.LookRotation(player.forward);
       
        Vector3 startPosition = player.position + startOffset;
        transform.position = startPosition;
        Vector3 targetPosition = startPosition + player.forward * _distance;
        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / _duration; // 진행 비율 (0~1)

            // 위치 업데이트
            if (!Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward.normalized, out RaycastHit hit, 0.5f) ||
                !hit.collider.CompareTag("Obstacle"))
            {
                transform.position = Vector3.Slerp(startPosition, targetPosition, t);
            }

            yield return null;
        }

        yield return new WaitForSeconds(_lifeTime);

        Managers.Resource.Destroy(gameObject);
    }
}
