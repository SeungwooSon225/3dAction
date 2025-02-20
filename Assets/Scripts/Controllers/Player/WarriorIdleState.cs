using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorIdleState : PlayerIdleState
{
    public WarriorIdleState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();  // 공통 Dodge 로직 수행

        _playerController.PlayerStat.RecoverMpStamina(1);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
