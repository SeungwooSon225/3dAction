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
        // 공격 애니메이션 트리거
        _playerController.Animator.SetTrigger("LeftShortClick");
        // 이동, 회피 등 다른 행동 방어
        _playerController.IsAttacking = true;
    }

    public override void OnUpdate()
    {
        Vector3 movementDir = Managers.Input.GetMovementInput();

        if (movementDir != Vector3.zero)
        {
            _playerController.MovementDir = movementDir;
        }

        // 공격 모션 중간에 상태를 계속 유지
        // 공격이 끝났는지 여부를 Animator 이벤트나 파라미터 등으로 체크
        if (!_playerController.IsAttacking)
        {
            // 공격 끝나면 Idle로 복귀 (단순 예시)
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }
}
