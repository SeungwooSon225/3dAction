using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerState _currentState;
    public PlayerState CurrentState { get { return _currentState; } set { _currentState = value; } }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerDodgeState DodgeState { get; private set; }

    public bool MouseLeftShortClick { get; set; }


    public PlayerStateMachine(PlayerController playerController)
    {
        IdleState = new PlayerIdleState(this, playerController);
        MoveState = new PlayerMoveState(this, playerController);
        AttackState = new PlayerAttackState(this, playerController);
        DodgeState = new PlayerDodgeState(this, playerController);

        _currentState = IdleState;
        _currentState.OnEnter();
    }

    public void ChangeState(PlayerState newState)
    {
        _currentState.OnExit();

        _currentState = newState;
        _currentState.OnEnter();
    }

    public void Update()
    {
        _currentState.OnUpdate();
    }
}
