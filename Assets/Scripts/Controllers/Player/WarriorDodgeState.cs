using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorDodgeState : PlayerDodgeState
{
    public WarriorDodgeState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();  // ���� Dodge ���� ����

        _playerController.PlayerStat.DisableAttackCollider("BasicComboOne");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
