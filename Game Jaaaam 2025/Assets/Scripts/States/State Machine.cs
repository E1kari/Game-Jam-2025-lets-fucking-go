using System.Data;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private BaseState currentState = new IdleState();

    private StateType currentStateType = StateType.IDLE;
    private StateType previousStateType = StateType.IDLE;



    void Start()
    {
        currentState.Enter(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
    }

    public void ChangeState(BaseState newState)
    {
        currentState.Exit();
        previousStateType = currentState.GetStateType();
        currentState = newState;
        currentStateType = currentState.GetStateType();
        currentState.Enter(this);
    }
}
