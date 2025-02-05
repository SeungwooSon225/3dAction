using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelStat : EnvObjectStat
{
    Map_Barrel _map_Barrel;

    void Start()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();

        _map_Barrel = transform.GetComponentInChildren<Map_Barrel>();

    }

    public override void OnAttacked(Attack attacker)
    {
        base.OnAttacked(attacker);

        Managers.AStar.IncreaseWeight(transform, 1);
    }

    protected override void OnDead(Attack attacker)
    {
        _map_Barrel.transform.parent = null;
        _map_Barrel.Explosion(attacker.IsPlayer);

        Managers.AStar.RemoveObstacle(transform);
        Managers.Resource.Destroy(_uI_EnvObjHPBar.gameObject);
        Managers.Resource.Destroy(gameObject);
    }
}
