using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine stateMachine, PlayerController playerController)
        : base(stateMachine, playerController) { }

    public override void OnEnter() 
    {
        _playerController.Animator.SetBool("IsRunning", false);
    }

    public override void OnUpdate() 
    {
        // 회피
        if (!_playerController.IsDodging && Input.GetKeyDown(KeyCode.Space) && 
            _playerController.PlayerStat.StaminaMp >= _playerController.PlayerStat.StaminaMpConsumption["Dodge"])
        {
            _stateMachine.ChangeState(PlayerStateType.Dodge);
            return;
        }

        // 마나, 스태미나 회복
        _playerController.PlayerStat.RecoverMpStamina(_playerController.GetIdleRecoverScale());

        // 스킬, 락온 입력 처리
        _stateMachine.HandleSkillEvent();
        _stateMachine.HandleLockOnEvent();

        // 움직임면 MoveState로 전환
        Vector3 movementDir = Managers.Input.GetMovementInput();
        if (movementDir != Vector3.zero)
        {
            _stateMachine.ChangeState(PlayerStateType.Move);
            return;
        }

        // 애니메이션에서 공격 이벤트가 발생하면 AttackState로 전환
        if (_playerController.IsAttacking) 
        {
            _stateMachine.ChangeState(PlayerStateType.Attack);
            return;
        }
    }
}
