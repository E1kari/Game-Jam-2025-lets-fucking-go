using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WalkingState : BaseState
{
    S_Player_Data playerData;
    StateType stateType = StateType.WALKING;

    StateMachine stateMachine;
    Rigidbody rb;
    InputAction moveAction;

    Animator animator;
    int direction = 2;
    private int lastDirection = 2;


    public void Enter(StateMachine pa_stateMachine)
    {
        Debug.Log("entered walking state");
        playerData = Resources.Load<S_Player_Data>("Scriptable Objects/S_Player_Data");

        stateMachine = pa_stateMachine;
        moveAction = InputSystem.actions.FindAction("Move");
        rb = stateMachine.gameObject.GetComponent<Rigidbody>();

        GameObject player = stateMachine.gameObject;
        GameObject child = player.transform.GetChild(0).gameObject;
        animator = child.GetComponent<Animator>();
    }

    public void UpdateState()
    {
        CheckExitConditions();

        Vector2 move = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector3(move.x * playerData.movementSpeed, 0, move.y * playerData.movementSpeed);

        animator.SetBool("change direction", false);
        DetermineDirection();

        if (direction != lastDirection)
        {
            lastDirection = direction;

            animator.SetInteger("walking animation", direction);
            animator.SetBool("change direction", true);
        }
    }

    public void DetermineDirection()
    {
        if (rb.linearVelocity.x == 0 && rb.linearVelocity.z == 0)
        {
            direction = 2;
        }
        else if (rb.linearVelocity.x <= rb.linearVelocity.z && rb.linearVelocity.x <= rb.linearVelocity.z * -1)
        {
            direction = 3; // links
        }
        else if (rb.linearVelocity.x >= rb.linearVelocity.z && rb.linearVelocity.x >= rb.linearVelocity.z * -1)
        {
            direction = 1; // rechts
        }
        else if (rb.linearVelocity.z <= rb.linearVelocity.x && rb.linearVelocity.z <= rb.linearVelocity.x * -1)
        {
            direction = 2; // vorne
        }
        else if (rb.linearVelocity.z >= rb.linearVelocity.x && rb.linearVelocity.z >= rb.linearVelocity.x * -1)
        {
            direction = 0; // hinten
        }
    }

    public void CheckExitConditions()
    {
        if (moveAction.ReadValue<Vector2>() == new Vector2(0, 0))
        {
            stateMachine.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
        animator.SetBool("exit", true);
    }

    public StateType GetStateType()
    {
        return StateType.WALKING;
    }
}
