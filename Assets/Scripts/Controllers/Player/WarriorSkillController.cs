using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkillController : MonoBehaviour
{
    WarriorController _warriorController;

    protected Dictionary<string, ParticleSystem> _effects = new Dictionary<string, ParticleSystem>();

    void Start()
    {
        _warriorController = GetComponent<WarriorController>();

        Transform skillE = Util.FindDeepChild(transform, "SkillE");
        if (skillE != null)
        {
            _effects.Add("SkillE", skillE.GetComponent<ParticleSystem>());
            skillE.transform.parent = null;
        }
        Transform skillR = Util.FindDeepChild(transform, "SkillR");
        if (skillR != null)
        {
            _effects.Add("SkillR", skillR.GetComponent<ParticleSystem>());
        }
    }

    private void PlayEffect(string effectName)
    {
        _effects[effectName].gameObject.transform.position = transform.position;
        _effects[effectName].gameObject.transform.rotation = Quaternion.LookRotation(transform.forward);
        _effects[effectName].Play();
    }

    private void ShootProjectile(string name)
    {
        GameObject projectile = Managers.Resource.Instantiate($"Projectiles/{name}");
        projectile.GetComponent<Projectile>().Shoot(_warriorController.PlayerStat);
    }
}
