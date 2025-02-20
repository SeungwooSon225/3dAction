using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerStateMachine stateMachine, PlayerController playerController)
        : base(stateMachine, playerController)
    {

    }

    public override void OnEnter()
    {
        Debug.Log("MoveState");
        _playerController.Animator.SetBool("IsRunning", true);
    }

    public override void OnUpdate()
    {
        if (!_playerController.IsDodging && Input.GetKeyDown(KeyCode.Space) && _playerController.PlayerStat.StaminaMp >= _playerController.PlayerStat.StaminaMpConsumption["Dodge"])
        {
            _stateMachine.ChangeState(PlayerStateType.Dodge);
            return;
        }

        _playerController.PlayerStat.RecoverMpStamina(_playerController.GetMoveRecoverScale());
        _stateMachine.HandleSkillEvent();
        _stateMachine.HandleLockOnEvent();

        Vector3 movementDir = Managers.Input.GetMovementInput();

        // 이동 입력이 없으면 Idle로 전환
        if (movementDir == Vector3.zero)
        {
            _stateMachine.ChangeState(PlayerStateType.Idle);
            return;
        }

        _playerController.MovementDir = movementDir;

        if (_playerController.IsAttacking)
        {
            _stateMachine.ChangeState(PlayerStateType.Attack);
            return;
        }

        _playerController.HandleMovement(movementDir);
    }

    public override void OnExit()
    {
        
    }
}
