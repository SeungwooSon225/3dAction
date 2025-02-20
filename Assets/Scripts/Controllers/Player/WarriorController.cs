using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorController : PlayerController
{
    public override PlayerDodgeState CreateDodgeState(PlayerStateMachine stateMachine)
    {
        return new WarriorDodgeState(stateMachine, this);
    } 
}
