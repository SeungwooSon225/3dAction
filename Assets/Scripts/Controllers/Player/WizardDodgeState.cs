using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardDodgeState : PlayerDodgeState
{
    WizardSkillController _wizardSkillController;

    public WizardDodgeState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController)
    {
        _wizardSkillController = playerController.GetComponent<WizardSkillController>();
    }

    public override void OnEnter()
    {
        base.OnEnter();  // ���� Dodge ���� ����

        _playerController.PlayerStat.SetOnAttackedResistFalse();
        _wizardSkillController.StopCharge();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
