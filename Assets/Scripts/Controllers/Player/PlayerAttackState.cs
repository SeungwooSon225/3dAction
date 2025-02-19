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
        _stateMachine.MouseLeftShortClick = false;
        // ���� �ִϸ��̼� Ʈ����
        _playerController.Animator.SetTrigger("LeftShortClick");
        // �̵�, ȸ�� �� �ٸ� �ൿ ���
        _playerController.IsAttacking = true;
    }

    public override void OnUpdate()
    {
        Vector3 movementDir = Managers.Input.GetMovementInput();

        if (movementDir != Vector3.zero)
        {
            _playerController.MovementDir = movementDir;
        }

        // ���� ��� �߰��� ���¸� ��� ����
        // ������ �������� ���θ� Animator �̺�Ʈ�� �Ķ���� ������ üũ
        if (!_playerController.IsAttacking)
        {
            // ���� ������ Idle�� ���� (�ܼ� ����)
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }
}
