using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(PlayerStateMachine stateMachine, PlayerController playerController)
    : base(stateMachine, playerController)
    {

    }

    public override void OnEnter()
    {
        Debug.Log("AttackState");
    }

    public override void OnUpdate()
    {
        if (!_playerController.IsDodging && Input.GetKeyDown(KeyCode.Space) && _playerController.PlayerStat.StaminaMp >= _playerController.PlayerStat.StaminaMpConsumption["Dodge"])
        {
            _stateMachine.ChangeState(PlayerStateType.Dodge);
            return;
        }

        Vector3 movementDir = Managers.Input.GetMovementInput();

        if (movementDir != Vector3.zero)
        {
            _playerController.MovementDir = movementDir;

            if (_playerController.Animator.GetBool("IsRunning") == false)
                _playerController.Animator.SetBool("IsRunning", true);
        }
        else 
        {
            if (_playerController.Animator.GetBool("IsRunning") == true)
                _playerController.Animator.SetBool("IsRunning", false);
        }

        // 공격 모션 중간에 상태를 계속 유지
        if (!_playerController.IsAttacking)
        {
            if (movementDir != Vector3.zero)
                _stateMachine.ChangeState(PlayerStateType.Move);
            else
                _stateMachine.ChangeState(PlayerStateType.Idle);
        }
    }
}
