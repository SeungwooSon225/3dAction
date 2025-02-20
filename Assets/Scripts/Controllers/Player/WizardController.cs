using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : PlayerController
{
    public override PlayerDodgeState CreateDodgeState(PlayerStateMachine stateMachine)
    {
        return new WizardDodgeState(stateMachine, this);
    }

    public override int GetIdleRecoverScale() { return 5; }
}
