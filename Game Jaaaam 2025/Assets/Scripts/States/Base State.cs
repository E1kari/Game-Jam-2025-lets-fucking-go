using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;

public enum StateType
{
    IDLE,
    WALKING
}

public interface BaseState
{
    public abstract void Enter(StateMachine pa_stateMachine);
    public abstract void UpdateState();
    public abstract void CheckExitConditions();
    public abstract void Exit();
    public StateType GetStateType();
}
