using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Barrel : MonoBehaviour
{
    Attack _attack;
    Collider _collider;

    ParticleSystem _playerExplosin;
    ParticleSystem _monsterExplosin;

    void Start()
    {
        _collider = GetComponent<Collider>();
        _attack = GetComponent<Attack>();
        _attack.Damage = 50f;
        _attack.AttackType = AttackType.Heavy;

        _playerExplosin = transform.Find("PlayerExplosion").GetComponent<ParticleSystem>();
        _monsterExplosin = transform.Find("MonsterExplosion").GetComponent<ParticleSystem>();
    }


    public void Explosion(bool isPlayer)
    {
        _attack.IsPlayer = isPlayer;
        if (isPlayer)
        {
            _playerExplosin.Play();
        }
        else
        {
            _monsterExplosin.Play();
        }

        StartCoroutine(ExplosionCo());
    }

    IEnumerator ExplosionCo()
    {
        _collider.enabled = true;

        yield return new WaitForSeconds(0.2f);

        _collider.enabled = false;
    }
}
