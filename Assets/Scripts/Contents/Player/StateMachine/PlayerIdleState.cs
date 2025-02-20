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
        // ȸ��
        if (!_playerController.IsDodging && Input.GetKeyDown(KeyCode.Space) && 
            _playerController.PlayerStat.StaminaMp >= _playerController.PlayerStat.StaminaMpConsumption["Dodge"])
        {
            _stateMachine.ChangeState(PlayerStateType.Dodge);
            return;
        }

        // ����, ���¹̳� ȸ��
        _playerController.PlayerStat.RecoverMpStamina(_playerController.GetIdleRecoverScale());

        // ��ų, ���� �Է� ó��
        _stateMachine.HandleSkillEvent();
        _stateMachine.HandleLockOnEvent();

        // �����Ӹ� MoveState�� ��ȯ
        Vector3 movementDir = Managers.Input.GetMovementInput();
        if (movementDir != Vector3.zero)
        {
            _stateMachine.ChangeState(PlayerStateType.Move);
            return;
        }

        // �ִϸ��̼ǿ��� ���� �̺�Ʈ�� �߻��ϸ� AttackState�� ��ȯ
        if (_playerController.IsAttacking) 
        {
            _stateMachine.ChangeState(PlayerStateType.Attack);
            return;
        }
    }
}
