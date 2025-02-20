using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine stateMachine, PlayerController playerController) 
        : base(stateMachine, playerController)
    { 
    
    }

    public override void OnEnter() 
    {
        Debug.Log("IdleState");
        _playerController.Animator.SetBool("IsRunning", false);
    }

    public override void OnUpdate() 
    {
        if (!_playerController.IsDodging && Input.GetKeyDown(KeyCode.Space) && _playerController.PlayerStat.StaminaMp >= _playerController.PlayerStat.StaminaMpConsumption["Dodge"])
        {
            _stateMachine.ChangeState(PlayerStateType.Dodge);
            return;
        }

        _playerController.PlayerStat.RecoverMpStamina(_playerController.GetIdleRecoverScale());
        _stateMachine.HandleSkillEvent();
        _stateMachine.HandleLockOnEvent();

        Vector3 movementDir = Managers.Input.GetMovementInput();
        if (movementDir != Vector3.zero)
        {
            _stateMachine.ChangeState(PlayerStateType.Move);
            return;
        }

        if (_playerController.IsAttacking) 
        {
            _stateMachine.ChangeState(PlayerStateType.Attack);
            return;
        }
    }
}
