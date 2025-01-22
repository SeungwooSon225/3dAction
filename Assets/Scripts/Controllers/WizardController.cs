using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : PlayerController
{
    void Update()
    {
        Moving();
        Skill();
    }

    protected override void Init()
    {
        base.Init();
    }
}
