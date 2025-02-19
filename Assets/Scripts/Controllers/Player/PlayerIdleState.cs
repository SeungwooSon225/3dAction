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
        _playerController.Animator.SetBool("IsRunning", false);
    }

    public override void OnUpdate() 
    {
        Vector3 movementDir = Managers.Input.GetMovementInput();
        if (movementDir != Vector3.zero)
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
            return;
        }

        if (_stateMachine.MouseLeftShortClick) 
        {
            _stateMachine.ChangeState(_stateMachine.AttackState);
            return;
        }
    }

    public override void OnExit() { }
}
