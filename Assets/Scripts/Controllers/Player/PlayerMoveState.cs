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
        _playerController.Animator.SetBool("IsRunning", true);
    }

    public override void OnUpdate()
    {
        Vector3 movementDir = Managers.Input.GetMovementInput();

        // 이동 입력이 없으면 Idle로 전환
        if (movementDir == Vector3.zero)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
            return;
        }

        _playerController.MovementDir = movementDir;

        if (_stateMachine.MouseLeftShortClick)
        {
            _stateMachine.ChangeState(_stateMachine.AttackState);
            return;
        }

        _playerController.HandleMovement(movementDir);
    }

    public override void OnExit()
    {
        _playerController.Animator.SetBool("IsRunning", false);
    }
}
