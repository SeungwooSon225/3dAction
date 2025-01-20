using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvObjectStat : Stat
{
    [SerializeField]
    UI_EnvObjHPBar _uI_EnvObjHPBar;

    void Start()
    {
        _isAttackable = true;
        _hp = 40;
        _maxHp = 40;
        _uI_EnvObjHPBar = transform.GetComponentInChildren<UI_EnvObjHPBar>();
    }

    protected override void OnDead(Attack attacker)
    {
        Managers.Resource.Destroy(gameObject); ;
    }

    public override void OnAttacked(Attack attacker)
    {
        base.OnAttacked(attacker);
        _uI_EnvObjHPBar.FadeInOut();
    }
}
