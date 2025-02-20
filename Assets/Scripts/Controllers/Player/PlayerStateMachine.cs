using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerStateType
{ 
    Idle,
    Move,
    Attack,
    OnAttacked,
    Dodge,
}

public class PlayerStateMachine : MonoBehaviour
{
    PlayerController _playerController;

    PlayerState _currentState;
    PlayerStateType _currentStateType;

    public PlayerState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public PlayerStateType CurrentStateType { get { return _currentStateType; } set { _currentStateType = value; } }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerOnAttackedState OnAttackedState { get; private set; }
    public PlayerDodgeState DodgeState { get; private set; }

    public PlayerStateMachine(PlayerController playerController)
    {
        _playerController = playerController;

        IdleState = new PlayerIdleState(this, playerController);
        MoveState = new PlayerMoveState(this, playerController);
        AttackState = new PlayerAttackState(this, playerController);
        OnAttackedState = new PlayerOnAttackedState(this, playerController);

        DodgeState = playerController.CreateDodgeState(this);

        _currentState = IdleState;
        _currentStateType = PlayerStateType.Idle;
        _currentState.OnEnter();
    }

    public void ChangeState(PlayerStateType newState)
    {
        _currentState.OnExit();
        _currentStateType = newState;

        switch (newState)
        {
            case PlayerStateType.Idle:          
                _currentState = IdleState;
                break;
            case PlayerStateType.Move:
                _currentState = MoveState;
                break;
            case PlayerStateType.Attack:
                _currentState = AttackState;
                break;
            case PlayerStateType.OnAttacked:
                _currentState = OnAttackedState;
                break;
            case PlayerStateType.Dodge:
                _currentState = DodgeState;
                break;
        }
        
        _currentState.OnEnter();
    }

    public void Update()
    {
        _currentState.OnUpdate();
    }

    public void HandleMouseEvent(Define.MouseEvent evt)
    {
        if (CurrentStateType == PlayerStateType.OnAttacked)
            return;

        switch (evt)
        {
            case Define.MouseEvent.LeftShortClick:
                if (_playerController.PlayerStat.StaminaMp >= _playerController.PlayerStat.StaminaMpConsumption["BasicAttack"])
                    _playerController.Animator.SetTrigger("LeftShortClick");
                break;
            case Define.MouseEvent.LeftLongClick:
                if (_playerController.PlayerStat.StaminaMp >= _playerController.PlayerStat.StaminaMpConsumption["ChargeAttack"])
                    _playerController.Animator.SetTrigger("LeftLongClick");
                break;
            case Define.MouseEvent.LeftClickUp:
                _playerController.Animator.SetTrigger("LeftClickUp");
                break;
        }
    }

    public void HandleSkillEvent()
    {
        Define.Skill skill = Managers.Input.GetSkillInput();

        switch (skill)
        {
            case Define.Skill.E:
                if (_playerController.PlayerStat.StaminaMp >= _playerController.PlayerStat.StaminaMpConsumption["SkillE"] && !Managers.UI.UI_PlayerStat.IsSkillECool)
                {
                    _playerController.Animator.SetTrigger("SkillE");
                    Managers.UI.UI_PlayerStat.IsSkillECool = true;
                }
                break;

            case Define.Skill.R:
                if (_playerController.PlayerStat.StaminaMp >= _playerController.PlayerStat.StaminaMpConsumption["SkillR"] && !Managers.UI.UI_PlayerStat.IsSkillRCool)
                {
                    _playerController.Animator.SetTrigger("SkillR");
                    Managers.UI.UI_PlayerStat.IsSkillRCool = true;
                }
                break;
        }
    }

    public void HandleLockOnEvent()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_playerController.PlayerStat.Target == null)
            {
                _playerController.PlayerStat.Target = Managers.Game.Monster.transform;
            }
            else
            {
                _playerController.PlayerStat.Target = null;
            }
        }    
    }
}
