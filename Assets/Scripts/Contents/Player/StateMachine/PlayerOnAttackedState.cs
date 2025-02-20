using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnAttackedState : PlayerState
{
    float elapsedTime;

    public PlayerOnAttackedState(PlayerStateMachine stateMachine, PlayerController playerController)
        : base(stateMachine, playerController)
    {

    }

    public override void OnEnter()
    {
        Debug.Log("OnAttackedState");
        elapsedTime = 0f;

        //_playerController.Animator.SetBool("IsAttacking", false);
        //_playerController.Animator.SetBool("IsDodging", false);
        _playerController.Animator.ResetTrigger("LeftShortClick");
        _playerController.Animator.ResetTrigger("LeftLongClick");
        _playerController.Animator.ResetTrigger("SkillE");
        _playerController.Animator.ResetTrigger("SkillR");
    }

    public override void OnUpdate()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > 0.5f && !_playerController.PlayerStat.IsDown)
        {
            Vector3 movementDir = Managers.Input.GetMovementInput();

            if (movementDir != Vector3.zero)
            {
                _playerController.MovementDir = movementDir;
                _playerController.Animator.SetBool("IsRunning", true);

                _stateMachine.ChangeState(PlayerStateType.Move);
            }
            else
            {
                _playerController.Animator.SetBool("IsRunning", false);

                _stateMachine.ChangeState(PlayerStateType.Idle);
            }
        }
    }
}
