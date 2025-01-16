using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    Stat _stat;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        _stat = gameObject.GetComponent<Stat>();
    }

    private void ShootProjectile(string name)
    {
        GameObject projectile = Managers.Resource.Instantiate($"Projectiles/{name}");
        projectile.GetComponent<Projectile>().Shoot(_stat);
    }
}

