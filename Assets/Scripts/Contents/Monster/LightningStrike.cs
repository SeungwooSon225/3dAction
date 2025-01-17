using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    [SerializeField]
    Attack _attack;
    [SerializeField]
    Collider _collider;


    public void Explosion()
    {
        _attack.Damage = 20f;
        _collider.enabled = false;
        StartCoroutine(ExplosionCo());
    }

    IEnumerator ExplosionCo()
    {
        yield return new WaitForSeconds(1f);

        _collider.enabled = true;

        yield return new WaitForSeconds(0.5f);

        _collider.enabled = false;
    }

}
