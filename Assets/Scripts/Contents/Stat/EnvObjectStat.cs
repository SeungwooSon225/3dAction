using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnvObjectStat : Stat
{
    protected UI_EnvObjHPBar _uI_EnvObjHPBar;
    Transform _effect;
    Collider _collider;

    void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        _hp = 3;
        _maxHp = 3;
        _uI_EnvObjHPBar = transform.GetComponentInChildren<UI_EnvObjHPBar>();
        _uI_EnvObjHPBar.Parent = transform;
        _uI_EnvObjHPBar.transform.parent = null;
        _effect = transform.Find("Effect");
        _collider = GetComponent<Collider>();
    }

    protected override void OnDead(Attack attacker)
    {
        Managers.AStar.RemoveObstacle(transform);

        if (_effect != null)
        {
            _effect.parent = null;
            _effect.GetComponent<ParticleSystem>().Play();
        }

        Managers.Resource.Destroy(_uI_EnvObjHPBar.gameObject);
        Managers.Resource.Destroy(gameObject);
    }

    public override void OnAttacked(Attack attacker)
    {
        Hp -= 1;

        StartCoroutine(InvicibleCo());

        if (Hp <= 0)
        {
            Hp = 0;
            OnDead(attacker);
        }

        _uI_EnvObjHPBar.FadeInOut();
    }

    IEnumerator InvicibleCo()
    {
        _collider.enabled = false;

        yield return new WaitForSeconds(0.5f);

        _collider.enabled = true;
    }
}
