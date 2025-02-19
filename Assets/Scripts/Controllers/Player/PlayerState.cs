using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    protected PlayerStateMachine _stateMachine;
    protected PlayerController _playerController;

    public PlayerState(PlayerStateMachine stateMachine, PlayerController playerController)
    {
        _stateMachine = stateMachine;
        _playerController = playerController;
    }

    public virtual void OnEnter() { }

    public virtual void OnUpdate() { }

    public virtual void OnExit() { }
}
