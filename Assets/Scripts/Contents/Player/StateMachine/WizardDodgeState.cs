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
        base.OnEnter();  // 공통 Dodge 로직 수행

        _playerController.PlayerStat.SetOnAttackedResistFalse();
        _wizardSkillController.StopCharge();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
