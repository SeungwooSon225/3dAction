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

    protected override void OnDead(Attack attacker)
    {
        _map_Barrel.transform.parent = null;
        _map_Barrel.Explosion(attacker.IsPlayer);

        Managers.AStar.RemoveObstacle(transform);
        Managers.Resource.Destroy(_uI_EnvObjHPBar.gameObject);
        Managers.Resource.Destroy(gameObject);
    }
}
