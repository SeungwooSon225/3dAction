using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowStorm : MonoBehaviour
{
    [SerializeField]
    float _radius;

    Transform _player;
    Animator _animator;
    Vector3 _previousSpawnPos = Vector3.zero;


    public void CastStorm(Transform player, Animator animator)
    {
        _player = player;
        _animator = animator;
        StartCoroutine(CastStormCo());
    }


    IEnumerator CastStormCo()
    {
        yield return new WaitForSeconds(.5f);
        

        for (int i=0; i<5; i++)
        {
            CastStorm();
            yield return new WaitForSeconds(.5f);

            CastStorm();
            yield return new WaitForSeconds(.5f);

            CastStormNearPlayer();
            yield return new WaitForSeconds(.5f);
        }

        _animator.SetBool("SnowStorm", false);
        gameObject.GetComponent<ParticleSystem>().Stop();
    }

    void CastStormNearPlayer()
    {
        float offsetX = Random.Range(0.5f, 1.5f);
        if (offsetX > 1f)
            offsetX *= -1;
        float offsetZ = Random.Range(0.5f, 1.5f);
        if (offsetZ > 1f)
            offsetZ *= -1;

        Vector3 spawnPosition = new Vector3(_player.position.x + offsetX, 0.1f, _player.position.z + offsetZ);

        _previousSpawnPos = spawnPosition;

        GameObject lightningStrikg = Managers.Resource.Instantiate("CrystalGuardian/LightningStrke");
        lightningStrikg.transform.position = spawnPosition;
        lightningStrikg.GetComponent<LightningStrike>().Explosion();
    }

    void CastStorm()
    {
        Vector2 randomPoint = Random.insideUnitCircle * _radius;
        Vector3 spawnPosition = new Vector3(gameObject.transform.position.x + randomPoint.x, 0.1f, gameObject.transform.position.z + randomPoint.y);

        if ((_previousSpawnPos - spawnPosition).magnitude < 5f)
        {
            float offsetX = Random.Range(10f, 12f);
            if (offsetX > 11f)
                offsetX *= -1;
            float offsetZ = Random.Range(10f, 12f);
            if (offsetZ > 11f)
                offsetZ *= -1;

            spawnPosition += new Vector3(offsetX, 0f, offsetZ);
        }

        _previousSpawnPos = spawnPosition;

        GameObject lightningStrikg = Managers.Resource.Instantiate("CrystalGuardian/LightningStrke");
        lightningStrikg.transform.position = spawnPosition;
        lightningStrikg.GetComponent<LightningStrike>().Explosion();
    }
}
