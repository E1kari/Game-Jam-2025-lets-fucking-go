using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : BaseState
{
    StateMachine stateMachine;
    StateType stateType = StateType.IDLE;

    public void Enter(StateMachine pa_stateMachine)
    {
        Debug.Log("entered idle state");
        stateMachine = pa_stateMachine;
    }

    // Update is called once per frame
    public void UpdateState()
    {
        CheckExitConditions();
    }

    public void CheckExitConditions()
    {
        if (InputSystem.actions.FindAction("Move").ReadValue<Vector2>() != new Vector2(0, 0))
        {
            stateMachine.ChangeState(new WalkingState());
        }
    }

    public void Exit()
    {

    }

    public StateType GetStateType()
    {
        return StateType.IDLE;
    }
}
