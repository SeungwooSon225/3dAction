using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerState
{
    public PlayerDodgeState(PlayerStateMachine stateMachine, PlayerController playerController)
    : base(stateMachine, playerController)
    {

    }

    public override void OnEnter()
    {
        Debug.Log("DodgeState");

        _playerController.IsDodging = true;

        _playerController.PlayerStat.StaminaMp -= _playerController.PlayerStat.StaminaMpConsumption["Dodge"];

        _playerController.Animator.SetTrigger("Dodge");
        _playerController.ResetClickTriggers();
        _playerController.StopMoveRotationCo();
        _playerController.transform.rotation = Quaternion.LookRotation(_playerController.MovementDir);
    }

    public override void OnUpdate()
    {
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

        if (!_playerController.IsDodging)
        {
            if (movementDir != Vector3.zero)
                _stateMachine.ChangeState(PlayerStateType.Move);
            else
                _stateMachine.ChangeState(PlayerStateType.Idle);            
        }
    }
}
